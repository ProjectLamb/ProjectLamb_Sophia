using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//List<IEnumerator> 보다 더 개선되고 더 많은 정보를 가진것.
//  비주얼 데이터도 가지고 있다.
//  VisualModifier을 여기로 분히하자.
//  타입또한 가지고 있다.

/// <summary>
/// List<IEnumerator> 대체제
/// 심지어 그냥 UnityAction 또한 가지고 있어라. 
/// </summary>
/// 
[System.Serializable]
public class AffectorPackage {
    [field : SerializeField] 
    public  STATE_TYPE affectorType;
    [HideInInspector] public List<IEnumerator>   AsyncAffectorCoroutine = null;
    [HideInInspector] public List<UnityAction>   Affector = null;

    public AffectorPackage(){
        this.AsyncAffectorCoroutine  = new List<IEnumerator>();
        this.Affector                = new List<UnityAction>();
    }
}