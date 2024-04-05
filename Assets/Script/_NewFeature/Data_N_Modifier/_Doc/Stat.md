### Defence
데미지 10퍼센트 감소하여 들어온다.

```mermaid
---
title : Defence
---
flowchart RL
    subgraph one
        direction LR
            Damage[Damage : 0.0~INF/ Base : 0.0] --> Function{Function}
            Defence[Defence : 0.0~?? / Base : 1.0 or 0.0] --> Function{Function}
            Function{Function} --> Result[RealDamage : 0.0~INF / Base : 0.0]
    end
    subgraph two
        direction LR
            EDam[100.0] --> EFun{ F? }
            EDef[ A? ] --> EFun{ ??? }
            EFun{ F? } --> ERes[90]
    end
```

### AttackSpeed

1. 공격속도는 10% 증가한다.
2. 애니메이션의 재생 속도 : 1.1배, 2면 2배가 될예정
    * 근접 공격속도는 애니메이션 재생 속도를 배속으로 
    * 원거리 공격속도는 1초당 투사체가 생산되는 속도를 배속으로 

```mermaid
---
title : Attack Speed
---
flowchart RL
    subgraph one
        direction LR
            PlaySpeed["`공격 모션 애니메이션 재생 속도 1배`"] --> Function{Function}
            AS["`AttackSpeed : 0 ~ INF / Base : 1 or 0.0f ?`"] --> Function{Function}
            Function{Function} --> Result[공격 모션 애니메이션 재생 속도 ? 배]
    end
    subgraph two
        direction LR
            EANS[1] --> EFun{ F? }
            EAS[ A? ] --> EFun{ ??? }
            EFun{ F? } --> ERes[1.1]
    end
```

### Tenacity
상태이상 저항력 
5초의 독데미지가 있다할때, 1초 감소하려면?

```mermaid
---
title : Tenecity
---
flowchart RL
    subgraph one
        direction LR
            In["5초동안 느려집니다. : 0~INF Base : 0"] --> F{Function}
            T["`Tenecity : 0 ~ INF / Base : 1 or 0.0f ?`"] --> F{Function}
            F{Function} --> Out[? 초동안 느려집니다.]
    end
    subgraph two
        direction LR
            EIN[5] --> EFun{ F? }
            ET[ A? ] --> EFun{???}
            EFun{ F? } --> EOut[4]
    end
```

```mermaid
---
title: Animal example
---

classDiagram

```