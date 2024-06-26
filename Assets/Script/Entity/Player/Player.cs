using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using MonsterLove.StateMachine;
using FMODPlus;

using Sophia;
using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

public class Player : Entity {

    /* 아래 4줄은 절때 활성화 하지마라. 상속받은 Entity에 이미 정의 되어 있다. */
    //Collider entityCollider;
    //Rigidbody entityRigidbody;
    //VisualModulator visualModulator;
    //GameObject model;

#region SerializeMember
    [SerializeField] public ScriptableObjPlayerData ScriptablePD;
    [SerializeField] private int mBarrierAmount;

#endregion

    //고유성을 가지고 있다는것이 특징이라서 Static하면 안되지 않을까?    

    public Sophia.Composite.DashSkill DashSkillAbility;
    
    public override ref EntityData GetFinalData(){
        return ref PlayerDataManager.GetFinalData().playerData.EntityDatas;
    }
    public override     EntityData GetOriginData(){
        return PlayerDataManager.GetOriginData().playerData.EntityDatas;
    }

    public override void ResetData(){
        PlayerDataManager.ResetFinal();
    }
    
    public WeaponManager            weaponManager;
    public SkillManager             skillManager;
    public EquipmentManager         equipmentManager;
    public ImageGenerator           imageGenerator;
    public LayerMask                groundMask; // 바닥을 인식하는 마스크
    public PlayerAnim               playerAnim;
    /// <summary>
    /// RoomGenerator.cs에서 참조하는 변수 (리펙토링 필요해 보인다.) <br/>
    /// * 포탈을 사용할수 있는지 없는지는 Map이 책임을 가져아 한다. <br/>
    /// * 플레이어는 그저 바닥에 포탈이 있는지 없는지 확인하고 사용하기, 안하기를 하면 될듯하다.
    /// </summary>

// 음직이는 방향을 얻어오는데 사용한다.
// 회전하는데 사용한다.
// 벽에 부딛혔는지 감지
// 대쉬를 했는지 
// 대쉬를 했는지 


    private Vector3                 mMoveVec;
    private Quaternion              mRotate;
    private bool                    mIsBorder;
    private bool                    mIsDashed;          
    
    public  bool                    IsExitAttack; // 공격 중 탈출가능시점
    public  bool                    DoAttackDash; // 공격하면서 앞으로 조금 대쉬하는 시점
    public  bool                    attackProTime; // 공격 이펙트 출현시점
    public  bool                    mIsDie; // 플레이어 사망여부
    public  bool                    mIsAttack; // 플레이어 공격여부

    [HideInInspector] Animator anim;
    
    private Vector2 inputVec;

    IEnumerator mCoWaitDash;        // StopCorutine을 사용하기 위해서는 코루틴 변수가 필요하다. 
    public ParticleSystem DieParticle;

    public enum PLAYERSTATES // 플레이어 상태 
    {
        Idle,
        Move,
        Attack,
        GetDamaged,
        Die,
    }   

    StateMachine<PLAYERSTATES> playerfsm;


    protected override void Awake(){
        /*아래 3줄은 절때 활성화 하지마라. base.Awake() 에서 이미 이걸 하고 있다.*/
        //if (!TryGetComponent<Collider>(out entityCollider)) { Debug.Log("컴포넌트 로드 실패 : Collider"); }
        //if (!TryGetComponent<Rigidbody>(out entityRigidbody)) { Debug.Log("컴포넌트 로드 실패 : Rigidbody"); }
        //if (!TryGetComponent<VisualModulator>(out visualModulator)) { Debug.Log("컴포넌트 로드 실패 : VisualModulator"); }
        //CurrentHealth = 마스터 데이터
        base.Awake();
        model.TryGetComponent<Animator>(out anim);
        model.TryGetComponent<PlayerAnim>(out playerAnim);

        //kabocha
        playerfsm = new StateMachine<PLAYERSTATES>(this);
        playerfsm.ChangeState(PLAYERSTATES.Idle);
      
        Life = new Sophia.Composite.LifeComposite(PlayerDataManager.GetEntityData().MaxHP);
    }

    private void Start() {
        DashSkillAbility = new Sophia.Composite.DashSkill(entityRigidbody, DashDataSender, 500);
        DashSkillAbility.SetAudioSource(DashSource);
        MasterData.MaxStaminaInject(DashSkillAbility.MaxStamina);
    }
    
#region 
    public override void GetDamaged(int _amount){
        if(Life.IsDie) {return;}
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damageAmount = _amount;
        damageInfo.damageRatio = 1;
        Life.Damaged(damageInfo);
        PlayerDataManager.GetEntityData().HitState.Invoke();
        anim.SetTrigger("GetDamaged");
        //ChangeState(PLAYERSTATES.GetDamaged);
        if(Life.CurrentHealth <= 0) Die(); // 체력이 0 이하면 사망 처리
    }

    public override void GetDamaged(int _amount, VFXObject obj){
        if(Life.IsDie) {return;}
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damageAmount = _amount;
        damageInfo.damageRatio = 1;
        Life.Damaged(damageInfo);
        PlayerDataManager.GetEntityData().HitState.Invoke();
        visualModulator.InteractByVFX(obj);
    }

    public override void Die() {
        mIsDie = true;
        //ChangeState(PLAYERSTATES.Die);
        anim.SetTrigger("Die");
    }

    public void OnMove(InputValue value) // new input system 사용
    {
        inputVec = value.Get<Vector2>();
    }

    public void Move() // new input system을 사용한 방식
    {
      Vector3 AngleToVector(float _angle) {
            _angle *= Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle));
        }

        float moveSpeed = PlayerDataManager.GetEntityData().MoveSpeed;
        
        if(DashSkillAbility.GetIsDashState()) { return; }
         
        anim.SetFloat("Move", entityRigidbody.velocity.magnitude);

        mMoveVec = AngleToVector(Camera.main.transform.eulerAngles.y + 90f) * inputVec.x + AngleToVector(Camera.main.transform.eulerAngles.y) * inputVec.y; // vaxis : inputvec.y , haxis : inputvec.x
        mMoveVec = mMoveVec.normalized;

        bool IsBorder(){return Physics.Raycast(transform.position, mMoveVec.normalized, 2, LayerMask.GetMask("Wall"));}
        
        if (!IsBorder()/*currentState != states[(int)PLAYERSTATES.Attack]*/) // 벽으로 막혀있지 않고 공격중이 아닐때만 이동 가능
        {
            Vector3 rbVel = mMoveVec * moveSpeed;
            this.entityRigidbody.velocity = rbVel;
            if(mMoveVec != Vector3.zero){
                //ChangeState(PLAYERSTATES.Move);
                mRotate = Quaternion.LookRotation(mMoveVec);
                transform.rotation = Quaternion.Slerp(transform.rotation,mRotate, 0.6f);
            }
            PlayerDataManager.GetEntityData().MoveState?.Invoke();
        }
    } 

#endregion

#region Rotation

    void Turning(UnityAction _turningCallback)
    {
        //100으로 해서 바닥을 인식 못했었다. 더 길게 하는게 좋다.
        float camRayLength = 500f;          // 씬으로 보내는 카메라의 Ray 길이
        // 마우스 커서에서 씬을 향해 발사되는 Ray 생성
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 레이캐스트 시작
        if (Physics.Raycast(camRay, out RaycastHit groundHit, camRayLength, groundMask)) // 공격 도중에는 방향 전환 금지
        {
            StartCoroutine(AsyncTurning(groundHit, _turningCallback));
        }
    }

    IEnumerator AsyncTurning(RaycastHit _groundHit, UnityAction _action){
        //yield return new WaitForEndOfFrame();
        Vector3 playerToMouse = _groundHit.point - transform.position;
        playerToMouse.y = 0f;
        Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);
        // 플레이어가 바라보는 방향 설정
        this.entityRigidbody.MoveRotation(newRotatation);
        yield return new WaitForEndOfFrame();
        _action.Invoke();
        yield return new WaitForEndOfFrame();
    }

#endregion

#region Dash
    public FMODAudioSource DashSource;
    
    public void Dash() => DashSkillAbility.Use();/*m*/ /*StatSpeed*/
    public (Vector3, int) DashDataSender() => (mMoveVec, (int)PlayerDataManager.GetEntityData().MoveSpeed);

#endregion

    public void Attack()
    {
        anim.SetTrigger("DoAttack");
        mIsAttack = true;
        playerfsm.ChangeState(PLAYERSTATES.Attack);
        //Turning(() => weaponManager.weapon.Use(PlayerDataManager.GetEntityData().Power));
    }
    
    public void AttackDash()
    {
        float camRayLength = 200f;
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);   
        if (Physics.Raycast(camRay, out RaycastHit hit, camRayLength)){
            Vector3 attackDash = hit.transform.position - this.transform.position;
            this.entityRigidbody.AddForce(attackDash*2.0f);
            }
        DoAttackDash = false; // 변수 초기화
        }
    /// <summary>
    /// Equipment_010과 의존 관계다 <br/>
    /// 슈슈슉 충전공격후 여러번 때리는것 <br/>
    /// 좋지 않은 구조니 하루빨리 개선사항을 고민하자. <br/>
    /// 이렇게 공격 방식이 바뀌는 매커니즘을 다루는 커플링을 줄일까? <br/>
    /// </summary>

    public void JustAttack(){
        Turning(() => {
            weaponManager.weapon.Use(PlayerDataManager.GetEntityData().Power);
        });
    }
    
    public void Skill(SKILL_KEY _key)
    {
        UnityAction TurnningCallback = () => {};
        switch (_key)
        {
            case SKILL_KEY.Q : 
                TurnningCallback = () => {skillManager.skills[0].Use(SKILL_KEY.Q, 0);};
                break;
            case SKILL_KEY.E : 
                TurnningCallback = () => {skillManager.skills[1].Use(SKILL_KEY.E, 0);};
                break;
            case SKILL_KEY.R : 
                TurnningCallback = () => {skillManager.skills[2].Use(SKILL_KEY.R, 0);};
                break;
        }
        Turning(TurnningCallback);
    }

    public void AimAssist()
    {
        float camRayLength = 200f;
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);   
        if (Physics.Raycast(camRay, out RaycastHit hit, camRayLength)){
            // 마우스가 "Enemy" 태그를 가진 collider와 충돌했을때
            if(hit.collider.tag == "Enemy"){
                // 공격중이라면
                if(mIsAttack){
                    // RAYCASTHIT가 닿은 대상의 중심을 바라본다
                    transform.rotation = Quaternion.LookRotation(hit.collider.transform.position - this.transform.position);
                    }
                }
            }
    }
    
    public void CheckAttack()
    {
        IsExitAttack = PlayerAnim.IsExitAttack;
        DoAttackDash = PlayerAnim.DoAttackDash;
        // 공격중이라면
        if(mIsAttack){
            anim.SetBool("IsAttack",true);
        }
        else{
            anim.SetBool("IsAttack",false);
        }

        //공격 중 이동이 감지되었다면
        if(IsExitAttack && inputVec != Vector2.zero)
        {
            //ChangeState(PLAYERSTATES.Idle);
            anim.SetBool("IsExitAttack",true);
        }
        else if(!IsExitAttack)
        {
            anim.SetBool("IsExitAttack",false);
        }

        if(!mIsAttack) // Idle 상태면 Attack 트리거 reset
            anim.ResetTrigger("DoAttack");
    }
}
