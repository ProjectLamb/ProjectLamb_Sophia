using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using UnityEngine.AI;
using DG.Tweening;

public class Robuwa : Enemy
{
    #region Public
    [Header("Mob Settings")]
    public int AttackRange;
    public int TurnSpeed;
    public int WanderingCoolTime;
    #endregion

    #region Private

    List<string> animBoolParamList;
    List<string> animTriggerParamList;
    private bool IsFirstRecog = true;
    private bool IsDie = false;

    #endregion
    // Start is called before the first frame update

    public enum States
    {
        Init,
        Idle,
        Move,
        Threat,
        Attack,
        Death,
    }

    StateMachine<States> fsm;

    FieldOfView fov;
    protected override void Awake()
    {
        base.Awake();

        animBoolParamList = new List<string>();
        animTriggerParamList = new List<string>();

        fsm = new StateMachine<States>(this);
        TryGetComponent<FieldOfView>(out fov);
        fsm.ChangeState(States.Init);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        CheckDeath();
        fsm.Driver.Update.Invoke();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        fsm.Driver.FixedUpdate.Invoke();
    }

    void CheckDeath()
    {
        if (Life.CurrentHealth <= 0 && !IsDie)
        {
            fsm.ChangeState(States.Death);
            IsDie = true;
        }
    }

    public override void Die()
    {
        base.Die();
        stage.mobGenerator.CurrentMobCount--;
        Invoke("DestroySelf", 0.5f);
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

    void DoAttack()
    {
        switch (animator.GetInteger("attackCount") % 3)
        {
            case 0:
                animator.SetTrigger("DoAttackLeft");
                break;
            case 1:
                animator.SetTrigger("DoAttackRight");
                break;
            case 2:
                animator.SetTrigger("DoAttackJump");
                break;
        }
    }

    void DoWander()
    {
        float range = GetComponent<FieldOfView>().viewRadius;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * range;

        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, range, -1);

        nav.destination = navHit.position;

        // currentTimer = 0;
        // IsWandering = true;
    }

    void SetCarrierBucketPosition()
    {
        carrierBucket.transform.position = transform.position + new Vector3(0, carrierBucket.GetComponent<CarrierBucket>().BucketScale / 2, AttackRange);
    }

    public void UseProjectile_NormalAttack()
    {
        this.carrierBucket.CarrierInstantiatorByObjects(this, projectiles[0], new object[] { FinalData.Power * 1 });
    }

    ////////////////////////////////////////FSM Functions////////////////////////////////////////
    /** Init State */
    void Init_Enter()
    {
        Debug.Log("Init_Enter");
        //Init Settings
        InitAnimParamList();
        SetCarrierBucketPosition();

        fsm.ChangeState(States.Idle);
    }

    void Idle_Enter()
    {
        Debug.Log("Idle_Enter");
        Freeze();
    }

    void Idle_Update()
    {
        if (!fov.IsRecog)
        {
            //Play Idle
            //DoWandering

        }
        else
        {
            if (IsFirstRecog)
                fsm.ChangeState(States.Threat);
            else
            {
                float dist = Vector3.Distance(transform.position, objectiveTarget.position);
                if (dist <= AttackRange)
                    fsm.ChangeState(States.Attack);
                else
                    fsm.ChangeState(States.Move);
            }
        }
    }

    /**Threat State*/
    void Threat_Enter()
    {
        Debug.Log("Threat Enter");

        Freeze();

        animator.SetTrigger("DoThreat");
    }

    void Threat_Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            IsFirstRecog = false;
            if (!fov.IsRecog)
                fsm.ChangeState(States.Idle);
            else
            {
                float dist = Vector3.Distance(transform.position, objectiveTarget.position);
                if (dist <= AttackRange)
                    fsm.ChangeState(States.Attack);
                else
                    fsm.ChangeState(States.Move);
            }
        }
    }

    void Threat_FixedUpdate()
    {
        transform.DOLookAt(objectiveTarget.position, TurnSpeed);
    }

    /**Move State*/

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
        else if (dist <= AttackRange)
            fsm.ChangeState(States.Attack);

    }

    void Move_FixedUpdate()
    {
        if (fov.IsRecog)
        {
            transform.DOLookAt(objectiveTarget.position, TurnSpeed);
            nav.SetDestination(objectiveTarget.position);
        }
    }

    void Move_Exit()
    {
        animator.SetBool("IsWalk", false);
    }

    /**Attack State*/
    void Attack_Enter()
    {
        Debug.Log("Attack_Enter");

        Freeze();

        transform.DOLookAt(objectiveTarget.position, TurnSpeed / 2);
        DoAttack();
    }

    void Attack_Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            fsm.ChangeState(States.Idle);
        }
    }

    void Attack_Exit()
    {
        ResetAnimParam();
    }

    /**Death State*/
    void Death_Enter()
    {
        Debug.Log("Death_Enter");
        Die();
    }

    void Death_Exit()
    {
        DestroySelf();
    }
}
