
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T mInstance;
    public static T Instance {
        get {
            if(mInstance == null){
                T[] assets = Resources.LoadAll<T>("");
                if(assets == null || assets.Length < 1){
                    throw new System.Exception("리소스에서 싱글톤 스크립터블 오브젝트 찾을수 없음");
                }
                else if(assets.Length > 1){
                    Debug.LogWarning("싱글톤 리소스가 1개 이상 감지되었습니다.");
                }
                mInstance = assets[0];
            }
            return mInstance;
        }
    }
}