using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace Sophia.Composite.RenderModels
{
    public class ModelHands : MonoBehaviour
    {
        [SerializedDictionary("Input Key", "GameObject")]
        private SerializedDictionary<E_MODEL_HAND, Transform> HandTransform = new SerializedDictionary<E_MODEL_HAND, Transform>(); // 무기 클래스를 가져온다.
        [SerializedDictionary("Input Key", "GameObject")]
        private SerializedDictionary<E_MODEL_HAND, GameObject> HoldingObject = new SerializedDictionary<E_MODEL_HAND, GameObject>(); // 무기 클래스를 가져온다.

        private void Awake()
        {
            GameObject ObjectFromTransform;
            if (!HandTransform.ContainsKey(E_MODEL_HAND.LeftHand))
            {
                HandTransform.Add(E_MODEL_HAND.LeftHand, transform.Find("LeftHandBucket"));
                if(HandTransform[E_MODEL_HAND.LeftHand].childCount != 0) {
                    ObjectFromTransform = HandTransform[E_MODEL_HAND.LeftHand].GetChild(0).gameObject;
                    HoldingObject.Add(E_MODEL_HAND.LeftHand, ObjectFromTransform);
                }
                else {HoldingObject.Add(E_MODEL_HAND.LeftHand, null);}
            }
            if (!HandTransform.ContainsKey(E_MODEL_HAND.RightHand))
            {
                HandTransform.Add(E_MODEL_HAND.RightHand, transform.Find("RightHandBucket"));
                if(HandTransform[E_MODEL_HAND.RightHand].childCount != 0) {
                    ObjectFromTransform = HandTransform[E_MODEL_HAND.RightHand].GetChild(0).gameObject;
                    HoldingObject.Add(E_MODEL_HAND.RightHand, ObjectFromTransform);
                }
                else{HoldingObject.Add(E_MODEL_HAND.RightHand, null);}
            }
        }

        public void HoldObject(GameObject go, E_MODEL_HAND handPos) {
            // 일단 트랜스폼과 그 트랜스폼의 게임오브젝트 생성 삭제 주기를 동일히 맞춰줘야 한다.
            // 홀딩 옵젝을 사용하기 전에 Hand트랜스폼에 물체가 있어야 하는것이 보장되야한다. 차일드로.
            // 보장한다고 가정하고 작업하면?
            // 홀딩옵젝이 일단 null인지 체크
                // 그거에 따라 있다면
                    // 현재있는 게임오브젝트 삭제
                    // 트랜스폼에 넣어주기
                // 없다면
                    // 트랜스폼에 넣어주기

            if(HoldingObject.TryGetValue(handPos, out GameObject holdObject)) {
               HoldingObject[handPos] = null;
               Destroy(holdObject); //트랜스폼 삭제
            }
            go.transform.SetParent(HandTransform[E_MODEL_HAND.RightHand]);
            HoldingObject[handPos] = go;
        }

        public void DropObject(E_MODEL_HAND handPos) {
            if(HoldingObject.TryGetValue(handPos, out GameObject holdObject)) {
               HoldingObject[handPos] = null;
               Destroy(holdObject); //트랜스폼 삭제
            }
        }
    }

}