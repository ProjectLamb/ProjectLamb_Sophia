using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 클래스 <br/>
/// * IDieAble : 죽는 Action , 인터페이스로 동작을 구현<br/>
/// * IDamagable : 맞는 Action , 인터페이스로 동작을 구현
/// </summary>
public class Enemy : MonoBehaviour, IDieAble, IDamagable
{
    [HideInInspector]
    EnemyData mEnemyData;

    public Transform target;
    NavMeshAgent nav;
    Rigidbody mRigidBody;

    public bool chase;
    public bool mIsDie;

    ///////////////////////////////////////////////////
    // public void HitEvent() {
    //     Camera.main.GetComponent<CameraEffect>().HandleZoomIn();
    //     GameManager.Instance.globalEvent.HandleTimeSlow();
    // }
    ///////////////////////////////////////////////////

    /// <summary>
    /// 인터페이스 구현
    /// </summary>
    public void Die()
    {
        chase = false;
        mRigidBody.velocity = Vector3.zero;
        Destroy(gameObject, 0.5f);
    }

    /// <summary>
    /// 인터페이스 구현ㅌ
    /// </summary>
    /// <param name="_amount"></param>
    public void GetDamaged(int _amount){
        mEnemyData.numericData.CurHP -= _amount;
        if (this.mEnemyData.numericData.CurHP <= 0) {this.Die();}
    }

    void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out mRigidBody)){Debug.Log("컴포넌트 로드 실패 : Rigidbody");}
        if(!TryGetComponent<EnemyData>(out mEnemyData)){Debug.Log("컴포넌트 로드 실패 : EnemyData");}
        if(!TryGetComponent<NavMeshAgent>(out nav)){Debug.Log("컴포넌트 로드 실패 : NavMeshAgent");}
        chase = false;
        target = GameManager.Instance?.playerGameObject?.transform;
        mIsDie = false;
    }

    void FixedUpdate()
    {
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}
        if (chase)
        {
            nav.SetDestination(target.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance?.globalEvent.IsGamePaused == true){return;}

        if (chase) { nav.enabled = true;}
        else {nav.enabled = false;}
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("EnterColider");
        if (collider.tag == "CombatEffect" && !mIsDie){
            GetDamaged(1);
            if(!collider.TryGetComponent<CombatEffect>(out CombatEffect combatEffect)){Debug.Log("컴포넌트 로드 실패 : NavMeshAgent");}
            Instantiate(combatEffect.hitEffect, transform);

            combatEffect.HitEvents.Invoke();
            //HitEvent();
        }
    }

    private void OnDestroy() {
        if(transform.parent == null) return;
        if(!transform.parent.TryGetComponent<RoomGenerator>(out RoomGenerator roomGenerator)){Debug.Log("컴포넌트 로드 실패 : NavMeshAgent");}
        roomGenerator.DecreaseCurrentMobCount();
    }
}
