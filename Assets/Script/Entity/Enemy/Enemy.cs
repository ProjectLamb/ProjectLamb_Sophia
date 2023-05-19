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
    public ScriptableObjEntityData scriptableObjEnemyData;
    EnemyData enemyData;
    Transform target;


    public NavMeshAgent nav;
    Rigidbody mRigidBody;

    public bool chase;
    public bool mIsDie;

    public void Die()
    {
        chase = false;
        mRigidBody.velocity = Vector3.zero;
        Destroy(gameObject, 0.5f);
    }

    public void GetDamaged(int _amount){
        enemyData.CurHP -= _amount;
        if (enemyData.CurHP <= 0) {this.Die();}
    }
    public void GetDamaged(int _amount, GameObject particle){
        enemyData.CurHP -= _amount;
        //
        if (enemyData.CurHP <= 0) {this.Die();}
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
        if(!TryGetComponent<NavMeshAgent>(out nav)){Debug.Log("컴포넌트 로드 실패 : NavMeshAgent");}
        enemyData = new EnemyData(scriptableObjEnemyData);
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
