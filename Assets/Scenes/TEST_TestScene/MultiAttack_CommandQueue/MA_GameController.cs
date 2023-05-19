using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
//owner이고, owner이 해야하는 동작을 여기서 정의해야한다.
public class MA_GameController : MonoBehaviour {
    //Monster
    MA_CommandQueue mMACommandQueue;
    public MA_TextData[] TD = new MA_TextData[3];
    public MA_TextAnime prefebs;
    private void Awake() {
        mMACommandQueue = new MA_CommandQueue();
    }

    private void Start() {
        StartCoroutine(Activator());
    }

    IEnumerator Activator(){
        yield return new WaitForSeconds(2f);

        mMACommandQueue.Enqueue(new RedAttackCmd(this));
        mMACommandQueue.Enqueue(new GreenAttackCmd(this));
        mMACommandQueue.Enqueue(new BlueAttackCmd(this));
    }
}