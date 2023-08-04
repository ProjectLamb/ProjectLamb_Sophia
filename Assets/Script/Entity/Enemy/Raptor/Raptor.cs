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

    int rushRange = 45;

    bool IsFirstRecog;
    bool IsWandering;
    bool IsChase;
    bool IsWalk;
    bool IsLook;
    // Start is called before the first frame update
    public override void Die()
    {
        animator.SetTrigger("DoDie");
        transform.parent.GetComponent<RaptorFlocks>().CurrentAmount--;
        base.Die();
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
        if (!IsFirstRecog)
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

        switch(direction)
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
        for(int i = 0; i < rf.smallAmount; i++)
        {
            if(rf.RaptorArray[i] != null)
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
        //Debug.Log(distance);
        if (distance <= rushRange)
        {
            IsChase = true;
            return;
        }
        animator.SetBool("IsRush", true);
    }
    protected override void Awake()
    {
        base.Awake();
        if (IsSmallRaptor)
        {
            isRecog = true;
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
        if(!IsSmallRaptor)
        {
            DoWandering();
        }
        else
        {
            Invoke("DoRush", Random.Range(0, 2));
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!IsSmallRaptor && GetComponent<FieldOfView>().IsRecog)
        {
            isRecog = true;
            if(!IsFirstRecog)
            {
                CancelInvoke();
                DoHowl();
                IsWandering = false;
                IsFirstRecog = true;
            }
        }
        else
        {
            isRecog = false;
        }

        if (!IsSmallRaptor)  //큰 개체
        {
            if (IsWandering)
            {
                currentTimer += Time.deltaTime;
                IsWalk = true;

                if (nav.remainingDistance <= nav.stoppingDistance)
                {
                    if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
                    {
                        Invoke("DoWandering", Random.Range(3, 6));
                        IsWandering = false;
                    }
                }
                if (currentTimer >= wanderingTime)
                {
                    Invoke("DoWandering", Random.Range(3, 6));
                    IsWandering = false;
                }
            }
            else
            {
                IsWalk = false;
            }
        }
        else  //작은 개체
        {
            if (IsChase)
            {
                float distance = Vector3.Distance(transform.position, objectiveTarget.position);
                IsWalk = true;

                if (distance <= rushRange + 5)
                {
                    IsChase = false;
                    DoRush();
                }
            }
            else
            {
                IsWalk = false;
            }

            if(animator.GetBool("IsTap"))
            {
                IsLook = true;
                nav.destination = objectiveTarget.position;
                //돌진 이펙트
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(IsWalk)
        {
            if (IsSmallRaptor)
                nav.destination = objectiveTarget.position;
            nav.SetDestination(nav.destination);
            IsLook = true;
            animator.SetBool("IsWalk", true);
        }
        else
        {
            animator.SetBool("IsWalk", false);
            IsLook = false;
        }

        if(IsLook)
        {
            transform.LookAt(nav.destination);
        }
    }

    /*void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == GameManager.Instance.PlayerGameObject)
        {
            DoMelee();
            GameManager.Instance.PlayerGameObject.GetComponent<Player>().GetDamaged(FinalData.Power);
        }

    }*/
}
