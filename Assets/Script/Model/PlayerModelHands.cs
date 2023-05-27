using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class PlayerModelHands : MonoBehaviour
{
    string LEFT_HAND = "LeftHand";
    string RIGHT_HAND = "RightHand";
    [SerializedDictionary("Input Key", "GameObject")]
    public SerializedDictionary<string, Transform> Hands = new SerializedDictionary<string, Transform>(); // 무기 클래스를 가져온다.
    [SerializedDictionary("Input Key", "GameObject")]
    public SerializedDictionary<string, GameObject> HoldingObject = new SerializedDictionary<string, GameObject>(); // 무기 클래스를 가져온다.

    private void Awake() {
        if(!Hands.ContainsKey(LEFT_HAND)){
            Hands.Add(LEFT_HAND, transform.Find("LeftHandBucket"));
        }
        if(!Hands.ContainsKey(RIGHT_HAND)){
            Hands.Add(RIGHT_HAND, transform.Find("RightHandBucket"));
        }
    }
}
