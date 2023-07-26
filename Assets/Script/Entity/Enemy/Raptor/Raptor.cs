using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Sophia_Carriers;

public class Raptor : Enemy
{
    public Projectile[] AttackProjectiles;
    float animationWalkSpeed;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        animationWalkSpeed = 2.0f + FinalData.MoveSpeed * 0.01f;
    }
    void Start()
    {
        chase = true;
        animator.SetFloat("MoveSpeed", animationWalkSpeed);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (chase)
        {
            animator.SetBool("IsWalk", true);
        }
        else
        {
            animator.SetBool("IsWalk", false);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // if (chase)
        // {

        // }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == GameManager.Instance.PlayerGameObject)
        {
            DoMelee();
            GameManager.Instance.PlayerGameObject.GetComponent<Player>().GetDamaged(FinalData.Power);
        }

    }

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
        Freeze();
    }

    public void DoMelee()
    {
        animator.SetBool("IsMelee", true);
        transform.parent.GetComponent<RaptorFlocks>().AttackCount++;
    }

    public void UseProjectile_NormalAttack()
    {
        this.carrierBucket.CarrierInstantiatorByObjects(this, AttackProjectiles[0], new object[] { FinalData.Power * 1 });
    }

    [ContextMenu("평타", false, int.MaxValue)]
    void InstantiateProjectiles1()
    {
        //Find Instantiate On This Animator Events;
        animator.SetTrigger("DoAttack");
    }

    [ContextMenu("범위데미지", false, int.MaxValue)]
    void InstantiateProjectiles2()
    {
        //Find Instantiate On This Animator Events;
        animator.SetTrigger("DoJump");
    }
}
