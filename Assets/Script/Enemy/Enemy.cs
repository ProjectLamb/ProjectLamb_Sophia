using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// 적 클래스 <br/>
/// * IDieAble : 죽는 Action , 인터페이스로 동작을 구현<br/>
/// * IDamagable : 맞는 Action , 인터페이스로 동작을 구현
/// </summary>
public class Enemy : MonoBehaviour, IDieAble, IDamagable, IAffectable
{
    [HideInInspector]
    EnemyData mEnemyData;

    public Transform target;
    NavMeshAgent nav;
    Rigidbody mRigidBody;

    public bool chase;
    public bool mIsDie;

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
    public void GetDamaged(int _amount, GameObject particle){
        mEnemyData.numericData.CurHP -= _amount;
        //
        if (this.mEnemyData.numericData.CurHP <= 0) {this.Die();}
    }
    public void AffectHandler(List<UnityAction> _action){
        _action.ForEach(E => E.Invoke());
    }

    public void AsyncAffectHandler(List<IEnumerator> _coroutine){
        _coroutine.ForEach(E => StartCoroutine(E));
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

    private void OnDestroy() {
        if(transform.parent.parent == null) return;
        if(!transform.parent.parent.TryGetComponent<StageGenerator>(out StageGenerator roomGenerator)){Debug.Log("컴포넌트 로드 실패 : NavMeshAgent");}
        roomGenerator.DecreaseCurrentMobCount();
    }
}
