using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    /// <summary>
    /// 플레이어의 수치적 데이터를 가져온다
    /// </summary>
    [HideInInspector]
    public PlayerData playerData;
    public Weapon weapon;
    public bool isPortal;
    Rigidbody mRigidbody;
    Vector3 mMoveVec;
    Vector3 mRotateVec;

    bool mIsBorder;
    bool mIsDashed;
    IEnumerator mCoWaitDash;

    


    private void Awake() {
        if(!TryGetComponent<PlayerData>(out playerData)) {Debug.Log("컴포넌트 로드 실패 : PlayerData");}
        if(!TryGetComponent<Rigidbody>(out mRigidbody)) {Debug.Log("컴포넌트 로드 실패 : Rigidbody");}
        
        weapon.playerData = this.playerData;

        isPortal = true;
    }

    /// <summary>
    /// 이 함수가 실행되면 프레임 마다 캐릭터가 음직인다.
    /// </summary>
    /// <param name="_hAxis">W, S 인풋 수치</param>
    /// <param name="_vAxis">A, D 인픗 수치</param>
    public void Move(float _hAxis, float _vAxis){
        if (mRigidbody.velocity.magnitude > playerData.numericData.MoveSpeed) return;

        mMoveVec = AngleToVector(Camera.main.transform.eulerAngles.y + 90f) * _hAxis + AngleToVector(Camera.main.transform.eulerAngles.y) * _vAxis;
        mMoveVec = mMoveVec.normalized;
        mRotateVec = new Vector3(_vAxis, 0, -_hAxis).normalized;
        if(!IsBorder()){
            Vector3 rbVel = mMoveVec * playerData.numericData.MoveSpeed;
            mRigidbody.velocity = rbVel;
            transform.LookAt(transform.position + mRotateVec);
        }
    }

    /// <summary>
    /// 이 함수가 실행되면 대쉬가 된다.
    /// </summary>
    public void Dash(){
        if(playerData.numericData.CurStamina <= 0) return; 
        Vector3 dashPower = mMoveVec * -Mathf.Log(1 / mRigidbody.drag);
        mRigidbody.AddForce(dashPower.normalized * playerData.numericData.MoveSpeed * 10, ForceMode.VelocityChange);
        Debug.Log($"Do Dash : Power:{dashPower}, Intencity:{mRigidbody.velocity.magnitude}");
        if (playerData.numericData.CurStamina > 0) {playerData.numericData.CurStamina--;}
        if (!mIsDashed) {
            mCoWaitDash = CoWaitDash();
            StartCoroutine(mCoWaitDash);
        }
    }

    public void Attack(){
        weapon.Use();
    }
    
    private IEnumerator CoWaitDash()
    {
        mIsDashed = true;
        while (playerData.numericData.CurStamina < playerData.numericData.MaxStamina)
        {
            yield return new WaitForSeconds(3.0f);
            playerData.numericData.CurStamina++;
        }
        mIsDashed = false;
    }

    private bool IsBorder(){
        return Physics.Raycast(transform.position, mMoveVec.normalized, 2, LayerMask.GetMask("Wall"));
    }

    private Vector3 AngleToVector(float _angle){
        _angle *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle));
    }
}