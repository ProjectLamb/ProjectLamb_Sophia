using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDieAble, IDamagable
{
    public Transform target;
    NavMeshAgent nav;
    Rigidbody mRigidBody;

    public bool chase;
    public bool mIsDie;

    [HideInInspector]
    EnemyData mEnemyData;

    public void Die()
    {
        chase = false;
        mRigidBody.velocity = Vector3.zero;
        Destroy(gameObject, 0.5f);
    }

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
        target = GameManager.Instance?.Player?.transform;
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

            GameManager.Instance?.globalEvent.OnHitEvents?.Invoke();
        }
    }

    private void OnDestroy() {
        if(transform.parent == null) return;
        if(!transform.parent.TryGetComponent<RoomGenerator>(out RoomGenerator roomGenerator)){Debug.Log("컴포넌트 로드 실패 : NavMeshAgent");}
        roomGenerator.DecreaseCurrentMobCount();
    }
}
