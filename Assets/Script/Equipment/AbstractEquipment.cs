using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*********************************************************************************
*
* 먹었을떄 적용되며, 해제될떄 적용 될것
*
*********************************************************************************/

//추후 스크립터블 오브젝트를 삭제하는것을 생각해보자..
// 이유는 웨펀 추가 클래스 만들고 
    // -> 웨펀 프리펩 만들고 
    // -> 스크립터블만들고 
    // -> 스크립터블 웨폰에 넣고 이 과정이 너무 복잡하다.
    // 어차핀 웨폰의 다형성은 프리펩과 상속 클래스로 실현되므로 너무 투머치


public abstract class AbstractEquipment {
    public ScriptableObjEquipmentData scriptableEquipment;
    public bool mIsApplyed;

    public virtual void Equip(ref PipelineData pd){}
    public virtual void Unequip(ref PipelineData pd){}
}