using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using DG.Tweening;
using Feature_NewData;

public class ElderOne : Boss
{
    List<string> animBoolParamList;
    List<string> animTriggerParamList;

    [Header("Stats")]
    public int attackRange = 30;
    public int attackCount = 3;

    private int turnSpeed = 2;
    private string currentAnimationName;
    private bool isPhaseChanged = false;


    // Start is called before the first frame update
    public enum States
    {
        Init,
        Idle,
        Move,
        Attack,
        Death,
    }

    StateMachine<States> fsm;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        animBoolParamList = new List<string>();
        animTriggerParamList = new List<string>();

        fsm = new StateMachine<States>(this);
        fsm.ChangeState(States.Init);
    }

    protected override void Update()
    {
        base.Update();

        CheckDeath();
        if(!isPhaseChanged)
            CheckPhase();

        fsm.Driver.Update.Invoke();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        fsm.Driver.FixedUpdate.Invoke();
    }

    void InitAnimParamList()
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter acp = animator.GetParameter(i);
            switch (animator.GetParameter(i).type)
            {
                case AnimatorControllerParameterType.Bool:
                    animBoolParamList.Add(acp.name);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animTriggerParamList.Add(acp.name);
                    break;
                default:
                    continue;
            }
        }
    }

    void ResetAnimParam()
    {
        foreach (string b in animBoolParamList)
            animator.SetBool(b, false);
        foreach (string t in animTriggerParamList)
            animator.ResetTrigger(t);
    }

    bool IsAnimationRunning(string s)
    {
        bool isRunning = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(s))
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime != 0 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                isRunning = true;

        return isRunning;
    }

    void CheckDeath()
    {
        if (CurrentHealth <= 0)
            fsm.ChangeState(States.Death);
    }

    void CheckPhase()
    {
        if(CurrentHealth <= MaxHealth / 2)
        {
            phase = 2;
            isPhaseChanged = true;
        }
    }

    void PlayRandomIdleAnimation(int idleAmount)
    {
        int random = Random.Range(0, idleAmount);
        switch (random)
        {
            case 0:
                animator.ResetTrigger(animTriggerParamList[0]);
                animator.ResetTrigger(animTriggerParamList[1]);
                break;
            case 1:
                animator.ResetTrigger(animTriggerParamList[1]);
                animator.SetTrigger(animTriggerParamList[0]);
                break;
            case 2:
                animator.ResetTrigger(animTriggerParamList[0]);
                animator.SetTrigger(animTriggerParamList[1]);
                break;
            default:
                ResetAnimParam();
                break;
        }
    }

    void DoAttack(int phase)
    {
        for (int i = 2; i <= 5; i++)
            animator.ResetTrigger(animTriggerParamList[i]);

        if (phase == 1)
        {
            int random = Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    animator.SetTrigger(animTriggerParamList[2]);
                    currentAnimationName = "EO_Attack_LeftPunch";
                    break;
                case 1:
                    animator.SetTrigger(animTriggerParamList[3]);
                    currentAnimationName = "EO_Attack_RightHook";
                    break;
                case 2:
                    animator.SetTrigger(animTriggerParamList[4]);
                    currentAnimationName = "EO_Attack_Continual";
                    break;
            }
        }
        else if (phase == 2)
        {
            animator.SetTrigger(animTriggerParamList[5]);
            currentAnimationName = "EO_Attack_Continual2";
        }
        else
        {
            Debug.Log("Normal Attack Error");
        }
    }

    void DoSkill(int phase)
    {
        for(int i = 2; i < animTriggerParamList.Count; i++)
            animator.ResetTrigger(animTriggerParamList[i]);
        
        if (phase == 1)
        {
            animator.SetTrigger(animTriggerParamList[6]);
            currentAnimationName = "EO_Attack_UpperCut";
        }
        else if (phase == 2)
        {
            if(animator.GetBool("phaseSkill"))
            {
                animator.SetTrigger(animTriggerParamList[8]);
                currentAnimationName = "EO_Attack_Phase2";
            }
            else
            {
                animator.SetTrigger(animTriggerParamList[7]);
                currentAnimationName = "EO_Attack_AttackWalkEnd";
            }
        }
    }

    public override void Die()
    {
        base.Die();
        //Death animation
    }

    ////////////////////////////////////////FSM Functions////////////////////////////////////////
    /** Init State */
    void Init_Enter()
    {
        Debug.Log("Init_Enter");
        //Init Settings
        InitAnimParamList();

        fsm.ChangeState(States.Idle);
    }

    /** Idle State */
    void Idle_Enter()
    {
        Debug.Log("Idle_Enter");
        ResetAnimParam();
    }

    void Idle_Update()
    {
        CheckDeath();
        if (!fov.IsRecog)
        {
            PlayRandomIdleAnimation(3);
        }
        //If HasTarget
        else
        {
            float dist = Vector3.Distance(transform.position, objectiveTarget.position);
            if (dist <= attackRange)
                fsm.ChangeState(States.Attack);
            else
                fsm.ChangeState(States.Move);
        }
    }

    /** Move State */
    void Move_Enter()
    {
        Debug.Log("Move_Enter");
        animator.SetBool("IsWalk", true);
        UnFreeze();
    }

    void Move_Update()
    {
        float dist = Vector3.Distance(transform.position, objectiveTarget.position);
        if (!fov.IsRecog)
            fsm.ChangeState(States.Idle);
        if (dist <= attackRange)
            fsm.ChangeState(States.Attack);
    }

    void Move_FixedUpdate()
    {
        transform.DOLookAt(objectiveTarget.position, turnSpeed);
        nav.SetDestination(objectiveTarget.position);
    }

    /** Attack State */
    void Attack_Enter()
    {
        Debug.Log("Attack_Enter");
        Freeze();

        transform.DOLookAt(objectiveTarget.position, turnSpeed / 2);

        //Skill
        if (animator.GetInteger("attackCount") == attackCount)
            DoSkill(phase);
        //Normal Attack
        else
            DoAttack(phase);
    }

    void Attack_Update()
    {
        if (!IsAnimationRunning(currentAnimationName))
            fsm.ChangeState(States.Idle);
    }

    void Attack_Exit()
    {
        UnFreeze();
    }

    /** Death State */
    void Death_Enter()
    {
        Debug.Log("Death_Enter");
        Die();
    }

    void Death_Update()
    {
        //check animation end
    }
    
    void Death_Exit()
    {
        DestroySelf();
    }
}