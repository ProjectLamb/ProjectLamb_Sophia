using System;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia
{
    public interface IFunctionalCommand<T> : IUserInterfaceAccessible {
        public void Invoke(ref T referer);
    }


    public interface IUserInterfaceAccessible {
        public string GetName();
        public string GetDescription();
        public Sprite GetSprite();
    }

    public interface IRandomlyActivatable {
        public bool GetIsActivated();
    }
    
}

/*

기존 상황

함수형 데이터라는 기획을 달성하기 위해. 동적으로 추가되는 동작의 구현이 필요성을 느겼다.
따라서 메소드를 변수처럼 다룰 수 있는 ( 🏞️ 델리게이트란?) 델리게이트를 사용하여, 함수를 동적으로 추가하는 방식으로 사용했다.
그리고 함수의 인수는 Call By Reference를 사용하여 인수 값에 영향을 미치도록 구현했음.

구현 동기

1 .Stat은 숫자형 원시형 자료형을 "사용한다" Extras는 함수 그룹을 사용한다. 이와같이.
Stat과 같이 어떻게 변경될지에 대한 숫자값이 존재 하는것 처럼 Extras 또한 그 숫자와 같은 상수과 같은,
원자성을 가지는 Function이 있어야 겠다고 판단했다.

2. 아이템, 스킬, 디버프를 구현할떄마다 비슷한 동작들이 있지만 서로 다른곳에서 정의한 함수를 통일 시키고 싶었다.
함수를 반복해서 정의할 필요 없이 전역적으로 접근해 재활용할 수 있는 함수를 가져다 사용해야 할 필요성을 느꼈다. 

3. 또한 "어떤 동작을 하는지에 대해" UI로 표시할 가능성이 존재한다. 뭐하는 추가동작인지에 대한 UI 를 표시하기 위해선, 
Function의 "Descrioption", "원시값으로 어떻게 변하는지에 대한 명세가 필요한것이다.

4. Sheet를 기입할때, 완벽하게 인덱스로 접근할 수 있도록 구현하는것이 목표다. 
최근 데이터 시트를 통한 문서 작업을 진행중이였기 떄문에다.

5. 마지막으로 델리게이트에서 사용해야 하는 멤버 데이터를 인수를 통해 전달할 수 없어 구현의 한계에 봉착하게 되었다.

해결법

IFunctionalCommand : UnityActionRef를 대체하는 인터페이스를 제작했음.

구현 상세
1. UnityAction의 Publish 방식을 모방하여, Invoke(ref T input)를 공개 메소드로 사용하도록 구현
2. IUserInterfaceAccessible를 구현하도록하여 뷰에 보여줘야 할 데이터를 접근할 수 있도록 인터페이스 구현하도록 함.

순서
1. 생성자를 통해서 FunctionalCommand내부 멤버를 초기화 하고
2. Invoke(ref T input)을 통해 함수 실행

의존관계
1. FunctionalCommandConcretes : 
    IFunctionalCommand를 추상 타입으로 인터페이스를 구현하는 구체화 된 펑셔널 커맨드로
    종류는 Entity가 가지는 Extras의 "원자적이며, 전역 접근이 가능한" 함수를 말한다.
    대표적으로 OnTriggerEnter(Collider) 타이밍에서는 실행되는 추가 동작으로서, 
    ref Entity를 받는 디버프/ 버프가 있다.

2. Extras & ExtrasModifier : 
    원자성을 가지는 함수인 "FunctionalCommand"의 Invoker다.
    그 원자성을 가진 함수는 3가지 수행(Perform)타입이 존재하는데.
    Start, Tick, Exit 타입이 존재한다. 
    이유는 Finate State Machine에서도 호완되도록 구현

3. Client & Affector, Equipments:
    클라이언트는 원자성을 가지는 함수인"FunctionalCommand"를 사용하는곳이다.
    즉, ExtrasModifier를 생성할때다. 생성자로 넣을때이다.
*/