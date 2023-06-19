using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TEST
{
    public class ArthmeticActions3 : MonoBehaviour
    {
        public static void func1(int i) { Debug.Log("1"); }
        public static void func2(int i) { Debug.Log("2"); }
        public static void func3(int i) { Debug.Log("3"); }
        public static void func4(int i) { Debug.Log("4"); }
        public static void func5(int i) { Debug.Log("5"); }

        TEST_ActionsSet actionsSet1 = new TEST_ActionsSet();
        TEST_ActionsSet actionsSet2 = new TEST_ActionsSet();
        private void Awake()
        {
            actionsSet1.AddAction(func1);
            actionsSet1.AddAction(func2);
            actionsSet1.AddAction(func3);
            actionsSet1.AddAction(func4);

            actionsSet2.AddAction(func2);
            actionsSet2.AddAction(func3);

            actionsSet1 -= actionsSet2;

            actionsSet1.ActionState.Invoke(100);
            //
            /*
            <예상했었던 결과는>
                0
                3
            <실제 결과>
                0
                3
            아~ 클래스 인스턴스마다 내부 함수의 힙 주소도 다른게 확실히 검증 되었다.
            그렇다면 외부 함수를 가지고 합 차 하는것이 맞구나~
            */
        }
    }
}