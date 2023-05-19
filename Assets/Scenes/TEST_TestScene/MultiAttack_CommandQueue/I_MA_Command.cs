using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 인터페이스는 public 할놈만 작성한다
// 그렇다면 아무리 공통이 있다 하더라도 외부 접근해서 안되는 애들은 
// 작성 안한다.
public interface I_MA_Command {
    UnityAction OnFinished {get;set;}
    void Execute();
}