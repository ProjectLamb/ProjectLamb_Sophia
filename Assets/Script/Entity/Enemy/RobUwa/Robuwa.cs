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
    public float WanderingCoolTime;
    #endregion

    #region Private

    private Vector3 wanderPosition;
    List<string> animBoolParamList;
    List<string> animTriggerParamList;
    private bool IsFirstRecog = true;
    private bool IsDie = false;
    private bool IsWandering = false;
    private float originViewRadius;

    #endregion
    // Start is called before the first frame update

    public enum States
    {
        Init,
        Idle,
        Move,
        Wander,
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
        stage.mobGenerator.RemoveMob(this.gameObject);
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
        EQS();

        float range = fov.viewRadius * 2;
        float minDistance = fov.viewRadius;
        Vector3 randomVector = Random.insideUnitSphere * range;
        NavMeshHit hit;

        randomVector += transform.position;

        if (minDistance > Vector3.Distance(randomVector, transform.position))
            DoWander();

        if (NavMesh.SamplePosition(randomVector, out hit, range, NavMesh.AllAreas))
        {
            wanderPosition = hit.position;
            IsWandering = true;
            fsm.ChangeState(States.Wander);
        }
        else
        {
            DoWander();
        }
    }

    void EQS()
    {
        //Environmental Query System
    }

    void SetCarrierBucketPosition()
    {
        carrierBucket.transform.position = transform.position + new Vector3(0, carrierBucket.GetComponent<CarrierBucket>().BucketScale / 2, AttackRange);
    }

    public void UseProjectile_NormalAttack()
    {
        this.carrierBucket.CarrierInstantiatorByObjects(this, projectiles[0], new object[] { FinalData.Power * 1 });
    }

    #region FSM Functions
    ////////////////////////////////////////FSM Functions////////////////////////////////////////
    /** Init State */
    void Init_Enter()
    {
        Debug.Log("Init_Enter");

        //Init Settings
        InitAnimParamList();
        SetCarrierBucketPosition();
        originViewRadius = fov.viewRadius;
        IsFirstRecog = true;
        spawnRate = 80.0f;

        fsm.ChangeState(States.Idle);
    }

    void Idle_Enter()
    {
        Debug.Log("Idle_Enter");
        fov.viewRadius = originViewRadius;
        Freeze();
    }

    void Idle_Update()
    {
        if (!fov.IsRecog)
        {
            //Play Idle
            if (!IsWandering)
                fsm.ChangeState(States.Wander);
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
        fov.viewRadius *= 2;
        animator.SetTrigger("DoThreat");
    }

    void Threat_Update()
    {
        //animator bool 바꾸기
        if (animator.GetBool("IsThreatEnd"))
        {
            IsFirstRecog = false;
            if (!fov.IsRecog)
            {
                fsm.ChangeState(States.Idle);
            }
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

    void Threat_Exit()
    {
        animator.SetBool("IsThreatEnd", false);
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

    /**Wander State*/
    void Wander_Enter()
    {
        System.Random random = new System.Random();
        Debug.Log("Wander_Enter");

        Invoke("DoWander", random.Next(0, 4));
        UnFreeze();
    }

    void Wander_Update()
    {
        if (fov.IsRecog)
        {
            CancelInvoke();
            fsm.ChangeState(States.Threat);
        }
        else if (IsWandering && nav.remainingDistance <= nav.stoppingDistance)
        {
            fsm.ChangeState(States.Idle);
        }
    }

    void Wander_FixedUpdate()
    {
        if (IsWandering)
        {
            animator.SetBool("IsWalk", true);
            transform.DOLookAt(wanderPosition, TurnSpeed);
            nav.SetDestination(wanderPosition);
        }
    }

    void Wander_Exit()
    {
        IsWandering = false;
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
    #endregion
}
