---
ebook:
  theme: one-dark.css
  title: 이벤트지향
  authors: Escatrgot
  disable-font-rescaling: true
  margin: [0.1, 0.1, 0.1, 0.1]
---
<style>
    h3.quest { font-weight: bold; border: 3px solid; color: #A0F !important;}
    .quest { font-weight: bold; color: #A0F !important;}

    h2 { border-top: 12px solid #D8D241; border-left: 5px solid #D8D241; border-right: 5px solid #D8D241; background-color: #D8D241; color: #FFF !important; font-weight: bold;}

    h3 { border-top: 3px solid #FFF; border: 2px solid #FFF; background-color: #FFF; color: #C4B000 !important;}

    h4 { font-weight: bold; color: #FFF !important; }
    .red{color: #d93d3d;}
    .darkred{color: #470909;}
    .orange{color: #cf6d1d;}
    .yellow{color: #DD3;}
    .green{color: #25ba00;}
    .blue{color: #169ae0;}
    .pink{color: #d10fd1;}
    .dim{color : #666666;}
    .lime{color : #addb40;}
</style>

## 다이나믹 아키텍쳐

### 서론

#### 이러한 아키텍쳐를 개발한 이유는 다음과 같다.

1. 클래스와 매서드를 분리 시켜 놓아 확장성 좋은 구조를 개발
2. 실행자가 가지고 있지 않는 함수를 대신 실행시키는 구조가 필요
3. 메서드를 에셋으로 저장 (스크립터블 오브젝트)

```
예를들어, UI 시스템에 영향을 미치는 Equipment


>> 다음의 생각 흐름 거쳤기에 생각한 아이디어다. <<

1. 상태이상, 버프, 시너지는 매쉬를 가지고 있고, 그 메쉬에 닿은 오브젝트는 그 효과를 받는것으로
    * 즉, Projectile을 통한 데이터 교환
    * 장점 : 프리펩과 시각 데이터를 적용할 수 있다.
    * 장점 : Projectile가 바로 인보커가 된다.
    * 단점 : Collider가 없는 객체는 Projectile의 영향을 받을 수 없다는것.
        * Solution : 카메라에 콜라이더를 만들어서 아주 큰 프로젝타일 생성 하면 되지 않을까?

2. 상태이상, 버프, 시너지는 장착한 장비에 의존적이다.
    아하 그럼 Equipment는 Projectile을 가지고 있구나.  
    Equipment는 여러개의 Projectile을 가질 수 있다.

3. 그런데 몬스터한테도 Equipment를 구현해야 하는건가?

Numeric & attribute
반 영구적, 영구적인것
```

#### 응용 디자인은 다음과 같다
##### \# 스크립터블 오브젝트 : readonly하며 공유되며, 불변하는 데이터 & 에셋 적용 가능

* [스크립터블 오브젝트 ](https://felipuss.tistory.com/entry/%EB%8B%88%EC%95%99%ED%8C%BD%EC%9D%B4-%EC%8A%A4%ED%81%AC%EB%A6%BD%ED%84%B0%EB%B8%94-%EC%98%A4%EB%B8%8C%EC%A0%9D%ED%8A%B8-%EA%B2%8C%EC%9E%84-%EA%B0%9C%EB%B0%9C-Unity-ScriptableObject)
* [플라이 웨이트 패턴](https://felipuss.tistory.com/entry/%EB%8B%88%EC%95%99%ED%8C%BD%EC%9D%B4-%EA%B0%9D%EC%B2%B4%EC%A7%80%ED%96%A5OOP-4-5-%EA%B5%AC%EC%A1%B0%ED%8C%A8%ED%84%B4-Flyweight-pattern)
##### \# 커맨드 패턴 : 실행자와 동작자를 분리 시켜놓는 디자인 
* [커맨드 패턴](https://felipuss.tistory.com/entry/%EB%8B%88%EC%95%99%ED%8C%BD%EC%9D%B4-%EA%B0%9D%EC%B2%B4%EC%A7%80%ED%96%A5OOP-4-6-%ED%96%89%EB%8F%99%ED%8C%A8%ED%84%B4-Command-pattern?category=951238)

##### \# Event Driven : 메소드를 객체화 시키는데 가장 핵심적인 기능
* [옵저버 패턴](https://felipuss.tistory.com/entry/%EB%8B%88%EC%95%99%ED%8C%BD%EC%9D%B4-%EA%B0%9D%EC%B2%B4%EC%A7%80%ED%96%A5OOP-4-6-%ED%96%89%EB%8F%99%ED%8C%A8%ED%84%B4-Observer-pattern)
* [UnityEvent](https://felipuss.tistory.com/entry/%EB%8B%88%EC%95%99%ED%8C%BD%EC%9D%B4-%EC%9D%B4%EB%B2%A4%ED%8A%B8C-5-UnityEvent)
* [Event Queue](https://github.com/Habrador/Unity-Programming-Patterns#14-event-queue)

##### \# 서브클래스 샌드박스 패턴 : 상속을 통한 중복되는 코드 제거 && 추상클래스를 통해 다형성이 높고, 범용성이 높은 클래스 개발 
* [서브클래스 샌드박스](https://felipuss.tistory.com/entry/%EB%8B%88%EC%95%99%ED%8C%BD%EC%9D%B4-%EA%B0%9D%EC%B2%B4%EC%A7%80%ED%96%A5OOP-4-6-%ED%96%89%EB%8F%99%ED%8C%A8%ED%84%B4-Command-pattern)

### 다이나믹 시스템
다이나믹 시스템은 
1. **"프리펩화"** 가 가능한 **"동적 작동 클래스"**
2. 구현자와 실행자가 분리되어 있다
   1. 구현자 : 반 영구성 vs 영구성
   2. 실행자 : 단발성 vs 즉발성
3. 다이나믹은 Affector의 참조 대상이 된다.
4. 모디파이어를 모두 이용할수는 없다. && 특정 메소드 에서만 사용 가능해야 한다.
```cs
using System;

Enum AffectorsState {
    Triggers ,Attack, GetHit, Die, ... 
};


Dictionary<AffectorsState, List<UnityAction>() DynamicsDictionry;

Numeric numericData;

void Attack(){
    Numeric AfteraffctedData;
    foreach(Func E in Dictionary[AffectorsState.Attack]){
        AfteraffctedData += E.Invoke(numericData);
    }

    weapon?.Use(AfteraffctedData, numericData);
}

void OnTriggerEnter(Trigger t) {
    t.DynamicsDictionry[AffectorState.Triggers].Invoke();
}

```

#### 1. Affector

|Player's Subclass|UI'Subclass|Monster'Subclass|
|:--|:--|:--|

1. Receiver 이면서, Invoker인 클래스다.
2. 외부 트리거에 의해 객체내 정의 안된 메소드를 실행시킬 수 있는 객체다.
3. Dynamics를 포함(참조) 하고 있는 객체다. 

#### 2. Modifier

모디 파이어는 다이나믹스를 참조하고 있는 
Affector의 데이터를 가져와 동적변화를 일으키는 함수다.

동적변화가 외부에서 정의 되므로 어디에서든 동작을 집어넣을 수 있는 "함수 확장성의 핵심이다."

<div align=center>
    <img src="2023-05-15-16-28-17.png" width=300px>
</div>

