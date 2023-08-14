using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Sophia_Carriers;

public class Raptor : Enemy
{
    public Projectile[] AttackProjectiles;
    public bool IsSmallRaptor;

    float animationWalkSpeed;

    float currentTimer;
    int wanderingTime = 5;
    int howlTime = 10;

    int rushRange = 50;
    float rushForce = 100f;
    float rushTime = 1.5f;

    bool IsFirstRecog;
    bool IsWandering;
    bool IsChase;
    bool IsWalk;
    bool IsLook;
    bool IsTapEnd;
    bool IsRushEnd;
    public bool IsRush;
    // Start is called before the first frame update
    protected override void NavMeshSet()
    {
        base.NavMeshSet();

        if (IsSmallRaptor)
        {
            nav.stoppingDistance = rushRange;
            nav.autoBraking = false;
        }

    }
    public override void Die()
    {
        animator.SetTrigger("DoDie");
        transform.parent.GetComponent<RaptorFlocks>().CurrentAmount--;
        base.Die();
        IsLook = false;
        nav.enabled = false;
    }

    public override void GetDamaged(int _amount)
    {
        animator.SetTrigger("DoHit");
        base.GetDamaged(_amount);
    }

    public override void GetDamaged(int _amount, VFXObject _vfx)
    {
        animator.SetTrigger("DoHit");
        base.GetDamaged(_amount, _vfx);
    }

    public void DoHowl()
    {
        animator.SetBool("IsHowl", true);
        if (!IsFirstRecog || transform.parent.GetComponent<RaptorFlocks>().CurrentAmount == 1)
            transform.parent.GetComponent<RaptorFlocks>().InstantiateSmallRaptor();
        else
            DoBuff();

        Invoke("DoHowl", Random.Range(howlTime - 1, howlTime + 3));
    }

    public void DoMelee()
    {
        animator.SetBool("IsMelee", true);
        transform.parent.GetComponent<RaptorFlocks>().AttackCount++;
    }

    void DoWandering()
    {
        int direction = Random.Range(0, 8);
        int range = Random.Range(25, 30);
        Vector3 destination = gameObject.transform.position;

        switch (direction)
        {
            case 0: //동
                destination = new Vector3(destination.x + range, destination.y, destination.z);
                break;
            case 1: //남동
                destination = new Vector3(destination.x + range / 2, destination.y, destination.z - range / 2);
                break;
            case 2: //남
                destination = new Vector3(destination.x, destination.y, destination.z - range);
                break;
            case 3: //남서
                destination = new Vector3(destination.x - range / 2, destination.y, destination.z - range / 2);
                break;
            case 4: //서
                destination = new Vector3(destination.x - range, destination.y, destination.z);
                break;
            case 5: //북서
                destination = new Vector3(destination.x - range / 2, destination.y, destination.z + range / 2);
                break;
            case 6: //북
                destination = new Vector3(destination.x, destination.y, destination.z + range);
                break;
            case 7: //북동
                destination = new Vector3(destination.x + range / 2, destination.y, destination.z + range / 2);
                break;
        }
        nav.destination = destination;
        currentTimer = 0;
        IsWandering = true;
    }

    void DoBuff()
    {
        //버프 주는 이펙트
        RaptorFlocks rf = transform.parent.GetComponent<RaptorFlocks>();
        for (int i = 0; i < rf.smallAmount; i++)
        {
            if (rf.RaptorArray[i] != null)
                rf.RaptorArray[i].GetComponent<Raptor>().GetBuff();
        }
    }

    public void GetBuff()
    {
        //버프 받기
        //FinalData.Power = BaseEnemyData.Power + 5;
        //FinalData.Defence = BaseEnemyData.Defence + 5;
        //Invoke("DeBuff", debuffTime);
    }

    void DoRush()
    {
        float distance = Vector3.Distance(transform.position, objectiveTarget.position);
        if (distance > rushRange)
        {
            IsChase = true;
            return;
        }
        nav.enabled = false;
        IsChase = false;
        IsWalk = false;
        currentTimer = 0;
        animator.SetBool("IsRush", true);
    }
    protected override void Awake()
    {
        base.Awake();
        if (IsSmallRaptor)
        {
            isRecog = true;
            IsLook = true;
            animationWalkSpeed = 2.0f + FinalData.MoveSpeed * 0.01f;
        }
        else
        {
            animationWalkSpeed = 1.5f + FinalData.MoveSpeed * 0.01f;
        }
    }
    void Start()
    {
        animator.SetFloat("MoveSpeed", animationWalkSpeed);
        if (!IsSmallRaptor)
        {
            DoWandering();
        }
        else
        {
            Invoke("DoRush", Random.Range(0f, 1f));
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!IsSmallRaptor)  //큰 개체
        {
            if(GetComponent<FieldOfView>().IsRecog) //첫 발견
            {
                isRecog = true;
                IsWandering = false;
                IsWalk = false;
                IsLook = true;
                if (!IsFirstRecog)
                {
                    CancelInvoke();
                    DoHowl();
                    IsFirstRecog = true;
                }
            }

            if (IsWandering)
            {
                currentTimer += Time.deltaTime;
                animator.SetBool("IsWalk", true);
                IsLook = true;

                if (nav.remainingDistance <= nav.stoppingDistance)
                {
                    if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
                    {
                        Invoke("DoWandering", Random.Range(3, 6));
                        animator.SetBool("IsWalk", false);
                        IsWandering = false;
                    }
                }
                if (currentTimer >= wanderingTime)
                {
                    Invoke("DoWandering", Random.Range(3, 6));
                    IsWandering = false;
                }
            }
        }
        else  //작은 개체
        {
            if (IsChase)
            {
                IsWalk = true;
                if (nav.remainingDistance <= nav.stoppingDistance)
                {
                    if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
                    {
                        IsChase = false;
                        DoRush();
                    }
                }
            }

            if (animator.GetBool("IsTap"))   //발 구를 때
            {
                IsLook = true;
                IsTapEnd = true;
            }
            if (!animator.GetBool("IsTap") && IsTapEnd)
            {
                IsLook = false;
                IsTapEnd = false;
            }

            if (!animator.GetBool("IsRush"))
            {
                if(IsRushEnd)
                {
                    Freeze();
                    nav.enabled = true;
                    IsChase = true;
                }
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsWalk)
        {
            nav.SetDestination(objectiveTarget.position);
            IsLook = true;
            animator.SetBool("IsWalk", true);
        }
        else
        {
            animator.SetBool("IsWalk", false);
        }

        if (IsLook)
        {
            if (!IsSmallRaptor && !IsFirstRecog)
                transform.LookAt(nav.destination);
            else
                transform.LookAt(objectiveTarget);
        }

        if(IsRush)
        {
            currentTimer += Time.deltaTime;
            UnFreeze();
            entityRigidbody.AddForce(transform.forward * rushForce, ForceMode.Acceleration);

            if(currentTimer >= rushTime)
            {
                IsRushEnd = true;
                animator.SetTrigger("DoRushQuit");
                IsRush = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == GameManager.Instance.PlayerGameObject)
        {
            GameManager.Instance.PlayerGameObject.GetComponent<Player>().GetDamaged(FinalData.Power);
        }
    }
}
