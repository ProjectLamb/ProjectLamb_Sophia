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

//앞으로 데이터 또한 MonoBehavuour로 하는것으로 하자
// 이전이랑 지금의 방식은 뭐가 다른것인가?
// 초창기 MONO Vs 지금 MONO
    // 초창기 : 상호 의존성이 너무 강했었다.
    // 지금 : 파이프라인이라는 글로벌 데이터를 가지는 것을 이용
// 스크립터블 VS 지금 스크립터블 폐기
    // 스크립터블 : 쓸데없는 데이터 저장과 쓸데없는 데이터를 다뤄야 했다
    // 지금 : 역시 프리펩이 최고다. 플라이웨이트와 같은 효과를 쓰려고 했는데
    //       그다지 플라이웨티으를 써서 이득볼 수 있는것이 없고
    //       이퀩, 무기, 이런것들은 오직 하나만 존재한다.


// 그렇다면 이 문제는 어떻게 해결하지?
// EntityData를 상속받는 이녀석
// public class PlayerData : EntityData {...}
    // 해결법은 EntityData 또한 MonoBehaviour를 상속받게하면 끝난다.

//그렇다면 총정리
    // 스크립터블을 폐기하자
    // 모든 데이터는 모노비헤이비어를 다시 상속받게한다.

//그렇게 함으로 개발자간 개발이 더 수월하게 함이 목적이 있다.
    // 다만 단점도 있다, 바로 인터페이스를 통한 소통간 매우 큰데이터를 가져오고 그럴것 같다.
    // 다만 인터페이스 소통이 포인터 데이터 8byte라면 사실 이것도 문제 없어 보인다.

//-------------------------------------//

//회의감 드는게 있다. 지금 Dynamics 시스템이 사실 프리펩으로도 해결 가능한거였었나?

//추후 스크립터블 오브젝트를 삭제하는것을 생각해보자..
// 이유는 웨펀 추가 클래스 만들고 
    // -> 웨펀 프리펩 만들고 
    // -> 스크립터블만들고 
    // -> 스크립터블 웨폰에 넣고 이 과정이 너무 복잡하다.
    // 어차핀 웨폰의 다형성은 프리펩과 상속 클래스로 실현되므로 너무 투머치


public abstract class AbstractEquipment : MonoBehaviour {
    public string equipmentName;
    public string description;
    public Sprite sprite;
    [SerializeField] public MasterData equipmentData;
    
    //[field : SerializeField] public AddingData addingData;    // 갑자기 여태 매개변수로 받다가 이걸 사용하는 이유.
    
    public UnityAction EquipState;
    public UnityAction UnequipState;
    public UnityAction UpdateState;
    public bool mIsInitialized = false;
    protected Player player;

    /*
        1. Base값을 가져오기 위한 수단
            2. Equip, Unequip 외의 Player 매개변수를 안받는 함수에서도 Base 값을 가져오기 위해서
            3. 심지어 State를 더하기 위해서 사용한다.

        굳이 플레이어가 아닐지라도 베이스 데이터를 가져와야 하는 날이 오지 않을까?
            나중에 생각해보자 일단
    */

    /// <summary>
    /// Equipment는 오직 플레이어만의 전유물이므로 언제든 Equipment의 owner는 너무 명확하다.
    /// </summary>

    public virtual void InitEquipment(Player _player, int _selectIndex){}
    public void InitEquipment(Player _player){this.InitEquipment(_player,0);}
}