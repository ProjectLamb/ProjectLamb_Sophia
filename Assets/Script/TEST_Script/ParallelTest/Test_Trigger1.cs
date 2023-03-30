using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Test_Trigger1 : MonoBehaviour
{
    bool Activate = false;
    Test_Controller test_Controller;
    private void Awake() {
        test_Controller = GetComponent<Test_Controller>();
    }

    private void Update() {
        if(test_Controller.Triggers == true && !Activate){
            Debug.Log(test_Controller.InvokeCount++);
            Activate = true;
        }

        if(test_Controller.Triggers == false){
            Activate= false;
        }
    }
}