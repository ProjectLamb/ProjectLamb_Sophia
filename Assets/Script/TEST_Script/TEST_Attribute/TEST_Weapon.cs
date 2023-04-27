using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Weapon : Weapon {
    List<GameObject> weaponProjectiles;
    public override void Use(){return;}

    public override IEnumerator CoWaitUse()
    {
        return null;
    }
    /*
        Attibute가 넣어졌어.
            Attribute란?
            수치 데이터가 아닌것,
            동작을 트리거 하는것이다.

            그럼 Attribute는 동작을 가지고 있는것인가? 트리거 하는것인가?
            고르라면 아무래도 동작을 자체적으로 할 수 있을것 같다.
        실행된다.
    */
}