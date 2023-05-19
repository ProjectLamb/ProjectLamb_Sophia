using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Sandbag : MonoBehaviour, IDieAble, IDamagable, IAffectable
{
    [field : SerializeField]
    private int mMaxHP;
        public int MaxHP {
            get{return mMaxHP;} 
            set{mMaxHP = value;}
        }

    [field : SerializeField]
    private int mCurHP;
        public int CurHP {
             get{return mCurHP;} 
             set{
                mCurHP = value;
                hpChangedEvent.Invoke();
            }
        }
    
    public int MoveSpeed {get;set;}

    public UnityEvent hpChangedEvent;
    public GameObject model;
    public ProjectileBucket projectileBucket;
    public ScriptableObjectEnemy sandbagData;

    public Transform LookAtTarget;
    public Projectile[] Projectiles;

    Animator animator;
    AnimEventInvoker animEventInvoker;
    VisualModulator visualModulator;
    bool mIsDie;
    /*********************************************************************************
    *
    *
    *
    *********************************************************************************/

    private void Awake() {
        model.TryGetComponent<Animator>(out animator);
        model.TryGetComponent<AnimEventInvoker>(out animEventInvoker);
        TryGetComponent<VisualModulator>(out visualModulator);
        this.MaxHP = sandbagData.MaxHP;
        this.CurHP = this.MaxHP;
        this.MoveSpeed = sandbagData.MoveSpeed;
        sandbagData.DeadParticle.GetComponent<ParticleCallback>().onDestroyEvent.AddListener(DestroySelf);
        LookAtTarget = GameManager.Instance.playerGameObject.transform;
    }
    private void Start() {
        Debug.Log("Catch");
        animEventInvoker.animCallback[(int)Enum_AnimState.Attack].AddListener( () => {
            projectileBucket.ProjectileInstantiator(Projectiles[0]);
        });
        animEventInvoker.animCallback[(int)Enum_AnimState.Jump].AddListener(() => {
            projectileBucket.ProjectileInstantiator(Projectiles[1]);
        });
    }

    public void GetDamaged(int _amount){
        if(mIsDie == true) {return;}
        this.CurHP -= _amount;
        animator.SetTrigger("DoHit");
        if (this.CurHP <= 0) {Die();}
    }

    public void GetDamaged(int _amount, GameObject particle){
        if(mIsDie == true) {return;}
        this.CurHP -= _amount;
        visualModulator.Interact(particle);
        animator.SetTrigger("DoHit");
        if (this.CurHP <= 0) {Die();}
    }

    public void Die(){ 
        mIsDie = true;
        GetComponent<Collider>().enabled = false;
        animator.SetTrigger("DoDie");
        visualModulator.vfxModulator.VFXInstantiator(sandbagData.DeadParticle);
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }

    public void AffectHandler(List<UnityAction> _Action) {
        _Action.ForEach(E => E.Invoke());
    }

    public void AsyncAffectHandler(List<IEnumerator> _Coroutine) {
        _Coroutine.ForEach(E => StartCoroutine(E));
    }

    private void Update() {
        if(!mIsDie)transform.LookAt(LookAtTarget);
    }

    [ContextMenu("평타", false, int.MaxValue)]
    void InstantiateProjectiles1(){
        //Find Instantiate On This Animator Events;
        animator.SetTrigger("DoAttack");
    }

    [ContextMenu("범위데미지",false, int.MaxValue)]
    void InstantiateProjectiles2(){
        //Find Instantiate On This Animator Events;
        animator.SetTrigger("DoJump");
    }
}