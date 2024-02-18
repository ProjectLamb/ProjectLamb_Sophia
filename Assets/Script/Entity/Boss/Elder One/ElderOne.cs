using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using DG.Tweening;
using Unity.VisualScripting;
using Sophia.Composite;

public class ElderOne : Boss, IRecogStateAccessible
{
    List<string> animBoolParamList;
    List<string> animTriggerParamList;
    public CarrierBucket attackCarrierBucket;

    [Header("Stats")]
    public int attackRange = 30;
    public int attackCount = 3;

    private int turnSpeed = 2;
    private bool isPhaseChanged = false;


    // Start is called before the first frame update
    public enum States
    {
        Init,
        Idle,
        Move,
        Attack,
        Skill,
        Invincible,
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
        if (!isPhaseChanged)
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

    void CheckDeath()
    {
        if (Life.CurrentHealth <= 0)
            fsm.ChangeState(States.Death);
    }

    void CheckPhase()
    {
        if (Life.CurrentHealth <= Life.MaxHp / 2)
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
                    break;
                case 1:
                    animator.SetTrigger(animTriggerParamList[3]);
                    break;
                case 2:
                    animator.SetTrigger(animTriggerParamList[4]);
                    break;
            }
        }
        else if (phase == 2)
        {
            animator.SetTrigger(animTriggerParamList[5]);
        }
        else
        {
            Debug.Log("Normal Attack Error");
        }
    }

    void DoSkill(int phase)
    {
        for (int i = 2; i < animTriggerParamList.Count; i++)
            animator.ResetTrigger(animTriggerParamList[i]);

        if (phase == 1)
        {
            animator.SetTrigger(animTriggerParamList[6]);
        }
        else if (phase == 2)
        {
            if (animator.GetInteger("phaseSkill") % 2 == 0)
            {
                animator.SetTrigger(animTriggerParamList[7]);
            }
            else
            {
                animator.SetTrigger(animTriggerParamList[8]);
            }
        }
    }

    public override void Die()
    {
        base.Die();
        //Death animation
    }

    void SetCarrierBucketPosition()
    {
        carrierBucket.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        attackCarrierBucket.transform.position = transform.position + new Vector3(0, attackCarrierBucket.GetComponent<CarrierBucket>().BucketScale / 2, attackRange);
    }

    public void UseProjectile_NormalAttack()
    {
        this.attackCarrierBucket.CarrierInstantiatorByObjects(this, projectiles[0], new object[] { FinalData.Power * 1 });
    }
    public void UseProjectile_JumpAttack()
    {
        this.carrierBucket.CarrierInstantiatorByObjects(this, projectiles[1], new object[] { FinalData.Power * 2 });
    }

    ////////////////////////////////////////FSM Functions////////////////////////////////////////
    /** Init State */
    void Init_Enter()
    {
        Debug.Log("Init_Enter");
        //Init Settings
        SetCarrierBucketPosition();
        InitAnimParamList();

        fsm.ChangeState(States.Idle);
    }

    /** Idle State */
    void Idle_Enter()
    {
        Debug.Log("Idle_Enter");
        ResetAnimParam();
        Freeze();
    }

    void Idle_Update()
    {
        if (this.recognize.GetCurrentRecogState() == Sophia.Composite.E_RECOG_TYPE.None)
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
        switch (recognize.GetCurrentRecogState()) 
        {
            case  Sophia.Composite.E_RECOG_TYPE.None : {
                fsm.ChangeState(States.Idle);
                break;
            }
            case  Sophia.Composite.E_RECOG_TYPE.FirstRecog : {
                float dist = Vector3.Distance(transform.position, objectiveTarget.position);
                if (dist <= attackRange)
                    fsm.ChangeState(States.Attack);
                break;
            }
            case  Sophia.Composite.E_RECOG_TYPE.Lose : {
                break;
            }
            case  Sophia.Composite.E_RECOG_TYPE.ReRecog : {
                float dist = Vector3.Distance(transform.position, objectiveTarget.position);
                if (dist <= attackRange)
                    fsm.ChangeState(States.Attack);
                break;
            }
        }
    }

    void Move_FixedUpdate()
    {
        transform.DOLookAt(objectiveTarget.position, turnSpeed);
        nav.SetDestination(objectiveTarget.position);
    }
    void Move_Exit()
    {
        animator.SetBool("IsWalk", false);
    }

    /** Attack State */
    void Attack_Enter()
    {
        Debug.Log("Attack_Enter");
        //Skill
        if (animator.GetInteger("attackCount") == attackCount)
            DoSkill(phase);
        //Normal Attack
        else
            DoAttack(phase);

        Freeze();

        transform.DOLookAt(objectiveTarget.position, turnSpeed / 2);
    }

    void Attack_Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("skill"))
                fsm.ChangeState(States.Skill);
            else
                fsm.ChangeState(States.Idle);
        }
    }

    void Skill_Enter()
    {
        Debug.Log("Skill_Enter");
        Freeze();
    }

    void Skill_FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("walk"))
        {
            transform.DOLookAt(objectiveTarget.position, turnSpeed);
            nav.SetDestination(objectiveTarget.position);
        }
    }

    void Skill_Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("walk"))
                fsm.ChangeState(States.Skill);
            else
                fsm.ChangeState(States.Idle);
        }
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
    
    public RecognizeEntityComposite GetRecognizeComposite() => this.recognize;
}