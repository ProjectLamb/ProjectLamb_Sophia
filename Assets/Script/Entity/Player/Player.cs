using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

public class Player : Entity {

    /* 아래 4줄은 절때 활성화 하지마라. 상속받은 Entity에 이미 정의 되어 있다. */
    //Collider entityCollider;
    //Rigidbody entityRigidbody;
    //VisualModulator visualModulator;
    //GameObject model;

    [SerializeField]
    public ScriptableObjPlayerData ScriptablePD;
    //고유성을 가지고 있다는것이 특징이라서 Static하면 안되지 않을까?    
    [SerializeField] private int mCurrentStamina;
    public  int CurrentStamina {
        get {return mCurrentStamina;}
        set {
            mCurrentStamina = value;
            if(mCurrentStamina < 0) {mCurrentStamina = 0;}
        }
    }
    public override ref EntityData GetFinalData(){
        return ref PlayerDataManager.GetEntityData();
    }
    public override     EntityData GetOriginData(){
        return PlayerDataManager.GetOriginData().playerData.EntityDatas;
    }

    public override void ResetData(){
        PlayerDataManager.ResetFinal();
    }
    
    [SerializeField]
    public Weapon weapon;

    [SerializeField]
    //public Skill[] skills;
    public EquipmentManager equipmentManager;
    public LayerMask groundMask;                  // 바닥을 인식하는 마스크
    public ImageGenerator imageGenerator;
    /// <summary>
    /// RoomGenerator.cs에서 참조하는 변수 (리펙토링 필요해 보인다.) <br/>
    /// * 포탈을 사용할수 있는지 없는지는 Map이 책임을 가져아 한다. <br/>
    /// * 플레이어는 그저 바닥에 포탈이 있는지 없는지 확인하고 사용하기, 안하기를 하면 될듯하다.
    /// </summary>

    public bool isPortal;
    Vector3 mMoveVec;               // 음직이는 방향을 얻어오는데 사용한다.
    Quaternion mRotate;             // 회전하는데 사용한다.
    bool mIsBorder;                 // 벽에 부딛혔는지 감지
    bool mIsDashed;                 // 대쉬를 했는지 
    bool mIsDie;                 // 대쉬를 했는지 
    public Animator anim;

    IEnumerator mCoWaitDash;        // StopCorutine을 사용하기 위해서는 코루틴 변수가 필요하다. 
    public ParticleSystem DieParticle;

    protected override void Awake(){
        /*아래 3줄은 절때 활성화 하지마라. base.Awake() 에서 이미 이걸 하고 있다.*/
        //if (!TryGetComponent<Collider>(out entityCollider)) { Debug.Log("컴포넌트 로드 실패 : Collider"); }
        //if (!TryGetComponent<Rigidbody>(out entityRigidbody)) { Debug.Log("컴포넌트 로드 실패 : Rigidbody"); }
        //if (!TryGetComponent<VisualModulator>(out visualModulator)) { Debug.Log("컴포넌트 로드 실패 : VisualModulator"); }
        base.Awake();
        //CurrentHealth = 마스터 데이터
    }

    private void Start() {
        CurrentHealth = PlayerDataManager.GetEntityData().MaxHP;//FinalPlayerData.PlayerEntityData.MaxHP;
        CurrentStamina = PlayerDataManager.GetPlayerData().MaxStamina;//FinalPlayerData.PlayerEntityData.MaxHP;
        isPortal = true;
        anim = model.GetComponent<Animator>();
    }
    
    public override void GetDamaged(int _amount){
        CurrentHealth -= (int)(_amount * 100/(100 + PlayerDataManager.GetEntityData().Defence));
        if(CurrentHealth <= 0) {Die();}
    }

    public override void GetDamaged(int _amount, VFXObject obj){
        //_amount의 값이 갑자기 바뀌어야 한다.
        //맞았을때 
        PlayerDataManager.GetEntityData().HitStateRef.Invoke(ref _amount);
        imageGenerator.GenerateImage(_amount);
        if(_amount == 0) return;
        
        CurrentHealth -= (int)(_amount * 100/(100+PlayerDataManager.GetEntityData().Defence));
        visualModulator.InteractByVFX(obj);
        if(CurrentHealth <= 0) {Die();}
    }

    public override void Die(){Debug.Log("죽었다는 로직 작성하기");}

    public void Move(float _hAxis, float _vAxis)
    {
        Vector3 AngleToVector(float _angle) {
            _angle *= Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle));
        }
        
        if (this.entityRigidbody.velocity.magnitude > PlayerDataManager.GetEntityData().MoveSpeed) return; 

        mMoveVec = AngleToVector(Camera.main.transform.eulerAngles.y + 90f) * _hAxis + AngleToVector(Camera.main.transform.eulerAngles.y) * _vAxis;
        mMoveVec = mMoveVec.normalized;

        bool IsBorder(){return Physics.Raycast(transform.position, mMoveVec.normalized, 2, LayerMask.GetMask("Wall"));}
        
        if (!IsBorder())
        {
            Vector3 rbVel = mMoveVec * PlayerDataManager.GetEntityData().MoveSpeed;
            this.entityRigidbody.velocity = rbVel;
            if(mMoveVec != Vector3.zero){
                mRotate = Quaternion.LookRotation(mMoveVec);
                transform.rotation = Quaternion.Slerp(transform.rotation,mRotate, 0.6f);
                
            }
            PlayerDataManager.GetEntityData().MoveState.Invoke();
        }
    }

    public void Dash()
    {
        IEnumerator CoWaitDash()
        {
            float recoveryTime = 3f - (3f * (PlayerDataManager.GetPlayerData().StaminaRestoreRatio / 100));
            mIsDashed = true;
            while (CurrentStamina < PlayerDataManager.GetPlayerData().MaxStamina)
            {
                yield return YieldInstructionCache.WaitForSeconds(recoveryTime);
                CurrentStamina++;
            }
            mIsDashed = false;
        }

        // 스테미나 false면 그냥 스킵
        if (CurrentStamina <= 0) return;

        Vector3 dashPower = mMoveVec * -Mathf.Log(1 / this.entityRigidbody.drag);
        this.entityRigidbody.AddForce(dashPower.normalized * PlayerDataManager.GetEntityData().MoveSpeed * 10, ForceMode.VelocityChange);

        if (CurrentStamina > 0) { CurrentStamina--; }

        if (!mIsDashed)
        {
            mCoWaitDash = CoWaitDash();
            StartCoroutine(mCoWaitDash);
        }
    }
    public void Attack()
    {
        anim.SetTrigger("DoAttack");
        Turning(() => weapon?.Use(PlayerDataManager.GetEntityData().Power));
        PlayerDataManager.GetEntityData().AttackState.Invoke();
    }
    
    /// <summary>
    /// Equipment_010과 의존 관계다 <br/>
    /// 슈슈슉 충전공격후 여러번 때리는것 <br/>
    /// 좋지 않은 구조니 하루빨리 개선사항을 고민하자. <br/>
    /// 이렇게 공격 방식이 바뀌는 매커니즘을 다루는 커플링을 줄일까? <br/>
    /// </summary>
    public void JustAttack(){
        Turning(() => { weapon?.Use(PlayerDataManager.GetEntityData().Power); });
    }
    
    // public void Skill(string key)
    // {
    //     if(skills[(int)E_SkillKey.Q]) {
    //         //this.FinalPlayerData.SkillState.Invoke();
    //         //Turning(() => skills[(int)E_SkillKey.Q].Use(this.FinalPlayerData.Power));
    //     }
    // }

    void Turning(UnityAction action)
    {
        //100으로 해서 바닥을 인식 못했었다. 더 길게 하는게 좋다.
        float camRayLength = 500f;          // 씬으로 보내는 카메라의 Ray 길이

        // 마우스 커서에서 씬을 향해 발사되는 Ray 생성
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Hit된 오브젝트의 정보를 담는것
        RaycastHit groundHit;

        // 레이캐스트 시작
        if (Physics.Raycast(camRay, out groundHit, camRayLength, groundMask))
        {
            // 마우스 눌린곳, 플레이어 위치 계산
            Vector3 playerToMouse = groundHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);
            // 플레이어가 바라보는 방향 설정
            this.entityRigidbody.MoveRotation(newRotatation);
            action.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag != "Equipment"){return;}
        AbstractEquipment triggerdEquipment = other.GetComponent<AbstractEquipment>();
        equipmentManager.Equip(triggerdEquipment);
        Debug.Log($"장비 장착! : {triggerdEquipment.equipmentName}");
        Destroy(triggerdEquipment.gameObject);
    }

}