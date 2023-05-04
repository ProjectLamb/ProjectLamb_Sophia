using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 플레이어의 모든 동작을 담는 클래스다
/// </summary>
public class PlayerAction : MonoBehaviour, IAffectableEntity
{
    /////////////////////////////////////////////////////////////////////////////////
    /*********************************************************************************
    *
    * 클래스를 모아 놓는다.
    *
    *********************************************************************************/

    [HideInInspector]
    public PlayerData playerData;           // 플레이어가 가지는 모든 데이터

    Rigidbody mRigidbody;

    /*********************************************************************************
    *
    * 플레이어 "액션"에 대해 필요한 변수들을 모아 놓는다.
    *
    *********************************************************************************/

    /// <summary>
    /// RoomGenerator.cs에서 참조하는 변수 (리펙토링 필요해 보인다.) <br/>
    /// * 포탈을 사용할수 있는지 없는지는 Map이 책임을 가져아 한다. <br/>
    /// * 플레이어는 그저 바닥에 포탈이 있는지 없는지 확인하고 사용하기, 안하기를 하면 될듯하다.
    /// </summary>

    public bool isPortal;
    Vector3 mMoveVec;               // 음직이는 방향을 얻어오는데 사용한다.
    Vector3 mRotateVec;             // 회전하는데 사용한다.
    bool mIsBorder;                 // 벽에 부딛혔는지 감지
    bool mIsDashed;                 // 대쉬를 했는지 
    Animator anim;
    GameObject model;

    public LayerMask groundMask;                  // 바닥을 인식하는 마스크


    /*********************************************************************************
    *
    * 코루틴, 액션을 모아 놓는다
    *
    *********************************************************************************/
    IEnumerator mCoWaitDash;        // StopCorutine을 사용하기 위해서는 코루틴 변수가 필요하다. 

    /*********************************************************************************
    *
    *
    *
    *********************************************************************************/
    void Awake()
    {
        if (!TryGetComponent<PlayerData>(out playerData)) { Debug.Log("컴포넌트 로드 실패 : PlayerData"); }
        if (!TryGetComponent<Rigidbody>(out mRigidbody)) { Debug.Log("컴포넌트 로드 실패 : Rigidbody"); }
        isPortal = true;

        foreach(E_DebuffState E in Enum.GetValues(typeof(E_DebuffState))){
            DebuffedDic.Add(E, false);
        }

        foreach(E_DebuffAtomic E in Enum.GetValues(typeof(E_DebuffAtomic))){
            AtomActivatorDic.Add(E, null);
        }
        model = transform.GetChild(0).gameObject;
        anim = model.GetComponent<Animator>();

    }

    void Update()
    {
    }

    /// <summary>
    /// 이 함수가 실행되면 프레임 마다 캐릭터가 음직인다.
    /// </summary>
    /// <param name="_hAxis">W, S 인풋 수치</param>
    /// <param name="_vAxis">A, D 인픗 수치</param>
    public void Move(float _hAxis, float _vAxis)
    {
        Vector3 AngleToVector(float _angle)
        {
            _angle *= Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle));
        }

        if (mRigidbody.velocity.magnitude > playerData.numericData.MoveSpeed) return;

        mMoveVec = AngleToVector(Camera.main.transform.eulerAngles.y + 90f) * _hAxis + AngleToVector(Camera.main.transform.eulerAngles.y) * _vAxis;
        mMoveVec = mMoveVec.normalized;
        mRotateVec = new Vector3(_vAxis, 0, -_hAxis).normalized;

        bool IsBorder()
        {
            return Physics.Raycast(transform.position, mMoveVec.normalized, 2, LayerMask.GetMask("Wall"));
        }

        if (!IsBorder())
        {
            Vector3 rbVel = mMoveVec * playerData.numericData.MoveSpeed;
            mRigidbody.velocity = rbVel;
            transform.LookAt(transform.position + mRotateVec);
        }
    }

    /// <summary>
    /// 이 함수가 실행되면 대쉬가 된다.
    /// </summary>
    public void Dash()
    {
        IEnumerator CoWaitDash()
        {
            mIsDashed = true;
            while (playerData.numericData.CurStamina < playerData.numericData.MaxStamina)
            {
                yield return YieldInstructionCache.WaitForSeconds(3.0f);
                playerData.numericData.CurStamina++;
            }
            mIsDashed = false;
        }

        // 스테미나 false면 그냥 스킵
        if (playerData.numericData.CurStamina <= 0) return;

        Vector3 dashPower = mMoveVec * -Mathf.Log(1 / mRigidbody.drag);
        mRigidbody.AddForce(dashPower.normalized * playerData.numericData.MoveSpeed * 10, ForceMode.VelocityChange);

        if (playerData.numericData.CurStamina > 0) { playerData.numericData.CurStamina--; }

        if (!mIsDashed)
        {
            mCoWaitDash = CoWaitDash();
            StartCoroutine(mCoWaitDash);
        }
    }

    /// <summary>
    /// Weapon 클래스를 참조해서 공격 을 실행한다.
    /// </summary>
    public void Attack()
    {
        anim.SetTrigger("DoAttack");
        Turning(() => playerData.weapon?.Use());
    }
    public void Skill(string key)
    {
        if(playerData.skills.ContainsKey("Q")) {Turning(() => playerData.skills["Q"].Use());}
    }

    /// <summary>
    /// 바닥에 레이케스트를 쏜다, 타일의 태그가 포탈이면 포탈에 해당하는 방이동(WarpPortal) 사용하기
    /// </summary>
    void Turning(UnityAction action)
    {
        float camRayLength = 100f;          // 씬으로 보내는 카메라의 Ray 길이
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
            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse) * Quaternion.Euler(0, -45, 0);
            // 플레이어가 바라보는 방향 설정
            mRigidbody.MoveRotation(newRotatation);
        }
        action.Invoke();
    }

    
    public void AffectHandler(UnityAction _action){
        _action.Invoke();
    }
    public void AsyncAffectHandler(IEnumerator _coroutine){
        StartCoroutine(_coroutine);
    }

    /*********************************************************************************
    *
    * 장비
    *
    *********************************************************************************/

    [ContextMenu("Equip All Equipments")]
    void Equip() { //적용이 되는지 확인만 하자
        foreach(Equipment E in playerData.equipments){
            IPlayerDataApplicant playerDataApplicant = (IPlayerDataApplicant)E;
            playerDataApplicant?.ApplyData(ref this.playerData);
        }
    }
    [ContextMenu("Dump All Equipments")]
    void Dump() { //적용이 되는지 확인만 하자
        foreach(Equipment E in playerData.equipments){
            IPlayerDataApplicant playerDataApplicant = (IPlayerDataApplicant)E;
            playerDataApplicant?.ApplyRemove(ref this.playerData);
        }
    }

    /*********************************************************************************
    *
    * 액티베이터 
    *
    *********************************************************************************/

    Dictionary<E_DebuffState, bool> DebuffedDic = new Dictionary<E_DebuffState, bool>();
    //Dictionary<E_DebuffState, IEnumerator> ActivatorDic = new Dictionary<E_DebuffState, IEnumerator>();
    Dictionary<E_DebuffAtomic, IEnumerator> AtomActivatorDic = new Dictionary<E_DebuffAtomic, IEnumerator>();

    IEnumerator DotCoroutine(float _durationTime, int _damAmount, E_DebuffState _debuffState) {
        while(_durationTime > 0){
            _durationTime -= 0.5f;
            playerData.numericData.CurHP -= _damAmount;
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }

    IEnumerator SlowCoroutine(float _durationTime, float _slowAmount, E_DebuffState _debuffState) {
        playerData.numericData.MoveSpeed *= _slowAmount;
        
        yield return YieldInstructionCache.WaitForSeconds(_durationTime);
        
        playerData.numericData.MoveSpeed /= _slowAmount;
    }

    IEnumerator UncontrollableCoroutine(float _durationTime, E_DebuffState _debuffState){

        PlayerController.IsAttackAllow = false;
        
        yield return YieldInstructionCache.WaitForSeconds(_durationTime);

        PlayerController.IsAttackAllow = true;

    }

    void Activate_DotDam_State(
        float _durationTime,
        int _damAmount,
        E_DebuffState _debuffState
        ) {
            AtomActivatorDic[E_DebuffAtomic.Dot] = DotCoroutine(_durationTime, _damAmount, _debuffState);
            StartCoroutine(AtomActivatorDic[E_DebuffAtomic.Dot]);
    }

    void Activate_Slow_State(
        float _durationTime,
        float _slowAmount,
        E_DebuffState _debuffState
        ) {
            AtomActivatorDic[E_DebuffAtomic.Slow] = SlowCoroutine(_durationTime, _slowAmount, _debuffState);
            StartCoroutine(AtomActivatorDic[E_DebuffAtomic.Slow]);
    }
    
    void Activate_Uncontrollable_State(float _durationTime, E_DebuffState _debuffState){
            AtomActivatorDic[E_DebuffAtomic.Uncontrollable] = UncontrollableCoroutine(_durationTime, _debuffState);
            StartCoroutine(AtomActivatorDic[E_DebuffAtomic.Uncontrollable]);
    }

    [ContextMenu("Burns")]
    public void Activate_Burn_state(
        string _strParams
        //float _durationTime,
        //int _damAmount
        ){
        string[] splitParams = _strParams.Split(',');
        Activate_DotDam_State(float.Parse(splitParams[0]), int.Parse(splitParams[1]), E_DebuffState.Burn);
    }

    [ContextMenu("Poisend")]
    public void Activate_Poisend_State(
        string _strParams
        //float _durationTime,
        //int _damAmount
        ){
        string[] splitParams = _strParams.Split(',');
        Activate_DotDam_State(float.Parse(splitParams[0]), int.Parse(splitParams[1]), E_DebuffState.Poisend);
    }

    [ContextMenu("Bleed")]
    public void Activate_Bleed_State(
        string _strParams
        //float _durationTime,
        //int _damAmount
        ){
        string[] splitParams = _strParams.Split(',');
        Activate_DotDam_State(float.Parse(splitParams[0]), int.Parse(splitParams[1]), E_DebuffState.Bleed);
    }

    [ContextMenu("Confused")]
    public void Activate_Confused_State(
        float _durationTime
        ){
        Activate_Uncontrollable_State(_durationTime, E_DebuffState.Confused);
    }

    [ContextMenu("Frearing")]
    public void Activate_Fearing_State(
        float _durationTime
        ){
        Activate_Uncontrollable_State(_durationTime, E_DebuffState.Fearing);
        Activate_Slow_State(_durationTime, 0.1f, E_DebuffState.Fearing);
    }
    [ContextMenu("Stern")]
    public void Activate_Stern_State(
        float _durationTime
        ){
        Activate_Uncontrollable_State(_durationTime, E_DebuffState.Stern);
        Activate_Slow_State(_durationTime, 0.0001f, E_DebuffState.Stern);
    }

    [ContextMenu("Bounded")]
    public void Activate_Bounded_State(
        float _durationTime
        ){
        Activate_Slow_State(_durationTime, 0.0001f, E_DebuffState.Bounded);
    }

    /*********************************************************************************
    *
    *
    *
    *********************************************************************************/
}