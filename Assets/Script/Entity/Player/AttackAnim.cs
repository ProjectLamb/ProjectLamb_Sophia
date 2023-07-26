using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    static public bool isAttack = false;
    // Start is called before the first frame update
    void attackStart(){
        isAttack = true;
        Debug.Log("now attack");
    }

    void attackEnd(){
        isAttack = false;
        Debug.Log("attack end");
    }
    
    public bool nowAttack(){
        return isAttack;
    }
}
