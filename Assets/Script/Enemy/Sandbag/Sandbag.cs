using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Sandbag : MonoBehaviour, IDieAble, IDamagable
{

    private int mMaxHP;
        public int MaxHP {
            get{return mMaxHP;} 
            set{mMaxHP = value;}
        }

    private int mCurHP;
        public int CurHP {
             get{return mCurHP;} 
             set{
                mCurHP = value;
                hpChangedEvent.Invoke();
            }
        }
    
    public int MoveSpeed {get;set;}

    public UnityAction hpChangedEvent;
    public VFXBucket vFXBucket;
    public ProjectileBucket projectileBucket;
    public ScriptableObjectEnemy sandbagData;
    public Animator animator;

    public bool mIsDie;
    public Transform LookAtTarget;
    public GameObject[] Projectiles;

    
    /*********************************************************************************
    *
    *
    *
    *********************************************************************************/

    private void Awake() {
        hpChangedEvent = () => {};
        this.MaxHP = sandbagData.MaxHP;
        this.CurHP = sandbagData.CurHP;
        this.MoveSpeed = sandbagData.MoveSpeed;
        sandbagData.DeadParticle.GetComponent<ParticleCallback>().onDestroyEvent.AddListener(DestroySelf);
    }

    public void GetDamaged(int _amount){
        this.CurHP -= _amount;
        animator.SetTrigger("DoHit");
        if (this.CurHP <= 0) {Die();}
    }

    [ContextMenu("GetDamaged", false, int.MaxValue)]
    public void GetDamaged(){
        //this.sandbagData.CurHP -= 334;
        CurHP -= 334;
        //this.hpChangedEvent.Invoke();
        //if (this.sandbagData.CurHP <= 0) {Die();}
        if (CurHP <= 0) {Die();}
    }
    // 공유 되는 데이터 이므로 인스턴스마다 차이나는 값을 지정해서는 안된다.
    //private void Update() {
    //    if (this.sandbagData.CurHP <= 0){Die();}
    //}

    public void Die(){ 
        mIsDie = true;
        animator.SetTrigger("DoDie");
        vFXBucket.VFXInstantiator(sandbagData.DeadParticle);
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log($"{gameObject.name} triggerEnter");
        if (collider.tag == "PlayerProjectile" && !mIsDie){
            Debug.Log($"{gameObject.name} Hit");
            if(collider.TryGetComponent<CombatEffect>(out CombatEffect projectile)){
                vFXBucket.VFXInstantiator(projectile.hitEffect);
                GetDamaged(100);
            }
        }
    }

    private void Update() {
        if(!mIsDie)transform.LookAt(LookAtTarget);
    }

    [ContextMenu("평타", false, int.MaxValue)]
    void InstantiateProjectiles1(){
        animator.SetTrigger("DoAttack");
    }

    [ContextMenu("범위데미지",false, int.MaxValue)]
    void InstantiateProjectiles2(){
        animator.SetTrigger("DoJump");
    }
}