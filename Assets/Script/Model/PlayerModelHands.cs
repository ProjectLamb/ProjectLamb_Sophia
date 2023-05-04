using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class PlayerModelHands : MonoBehaviour
{
    [SerializedDictionary("Input Key", "GameObject")]
    public SerializedDictionary<string, GameObject> Hands = new SerializedDictionary<string, GameObject>(); // 무기 클래스를 가져온다.

    private void Awake() {
        if(!Hands.ContainsKey("LeftHand")){
            Hands.Add("LeftHand", GameObject.Find("Bip001 L Hand"));
        }
        if(!Hands.ContainsKey("RightHand")){
            Hands.Add("RightHand", GameObject.Find("Bip001 R Hand"));
        }
    }
}
