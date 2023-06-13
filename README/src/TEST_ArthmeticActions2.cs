using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEST_ArthmeticActions2 : MonoBehaviour
{
    TEST_ActionsSet actionsSet1 = new TEST_ActionsSet();
    TEST_ActionsSet actionsSet2 = new TEST_ActionsSet();
    private void Awake() {
        actionsSet1.AddAction(0);
        actionsSet1.AddAction(1);
        actionsSet1.AddAction(2);
        actionsSet1.AddAction(3);

        actionsSet2.AddAction(1);
        actionsSet2.AddAction(2);

        actionsSet1 -= actionsSet2;

        actionsSet1.ActionState.Invoke(100);
        //결과는 함수가 빠지지 않게 된다
        /*
        <예상했었던 결과는>
           0
           3
        <실제 결과>
            0
            1
            2
            3

        비록 깉은 동작을 하는 함수지만,
        클래스가 다르가 -> 서로 다른 힙 메모리 주소를 가진다는것
        그말은 즉슨 클래스의 메소드 또한 다른 메모리 주소를 가지므로 그렇다.

        그렇다면 만약 공유되는 함수가 있다면. 그것은 합, 차 해도 상관이 없지 않을까?
        함수를 글로벌 하게 선언하고 작동시켜보자.
        */
    }
}
