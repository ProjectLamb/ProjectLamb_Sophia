using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    // 해당 상태를 시작할 때 1회호출
    public abstract void Enter(Player player);

    // 해당 상태를 업데이트할 때 매 프레임 호출
    public abstract void Update(Player player);

    // 해당 상태를 종료할 때 1회 호출
    public abstract void Exit(Player player);
}
