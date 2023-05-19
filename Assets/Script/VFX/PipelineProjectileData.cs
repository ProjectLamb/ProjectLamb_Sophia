using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PipelineData {
    // Equipmetn data의 합산
    // 최종의 데이터로 날아갈 놈 이걸 어떻게 정해야 할까?

    // 플레이어 데이터랑 최종 데이터의 차이점은 뭐지?

    // 스크립터블 오브젝트에게는 변화란 없다.
    // 모두 다 BASE 데이터가 된다.

    // 무기, 스킬, 장비의 (BASE데이터 제외) BASE를 상속받는 데이터는 모두 변동이 있을 여지가 있다를 가정해보자.
    // BASE를 상속 받는데 필요한 데이터도 있을것이다.

    // 파이프라인 데이터는 합 연산이 가능하다.
        //모두가 BASE인 상태를 가정.
        // 평타
            // 플레이어 동적 데이터 + 무기 동적 데이터 
            // 파이프라인이 

        public class AttackPipeData {
            //////////////////////////////////////////////////////
            public int MaxHP, CurHP, MaxStamina, CurStamina;
            public float MoveSpeed, Power, Luck, Defense, Tenacity;
            //////////////////////////////////////////////////////
            public float DamageRatio, WeaponDelay, Range;
            //////////////////////////////////////////////////////
        }
        // Data = BASEs.Foreach(AttackPipeData)  주로 Get만 한다
        
        // 스킬 사용했을때,
        public class SkillPipeData {
            //////////////////////////////////////////////////////
            public int MaxHP, CurHP, MaxStamina, CurStamina;
            public float MoveSpeed, Power, Luck, Defense, Tenacity;
            //////////////////////////////////////////////////////
            public float DamageRatio, WeaponDelay, Range;
            //////////////////////////////////////////////////////
            public float SkillDelay, durateTime;
        }
        // 플레이어 동적 데이터 + 무기 동적 데이터 + 스킬
        // Get도 하는데 Set또한 한다.
            // Gets 
                // Data =BASEs.Foreach(SkillPipeData)  + 를한다.
            // Sets
                // BASEs.Foreach(SkillPipeData) to origin; 
        
        // 장비를 얻었을때.
        public class EquipmentPipeData {
            //////////////////////////////////////////////////////
            public int MaxHP, CurHP, MaxStamina, CurStamina;
            public float MoveSpeed, Power, Luck, Defense, Tenacity;
            //////////////////////////////////////////////////////
            public float DamageRatio, WeaponDelay, Range;
            //////////////////////////////////////////////////////
            public float SkillDelay, durateTime;
            //////////////////////////////////////////////////////
            public Dictionary<Affector_PlayerState, UnityAction> dynamicsDic;
        }
        // Get도 하는데 Set또한 한다.
            // Gets 예를들어 체력 비례 데미지를 준다.
                // Data =BASEs.Foreach(SkillPipeData)  + 를한다.
            // Sets 
                // BASEs.Foreach(SkillPipeData) to origin; 
        // 장비를 버렸을때,

//파이프라인의 변경은 
//평타 사용시,
//버프 얻을때
//얻고 나서
//투사체 생성

    // 플레이어 데이터는 스크립터블 오브젝트
        // 플레이어에게만 있다.
        // 플레이어는 동적인 데이터도 있다.
    // 무기 데이터도 스크립터블 오브젝트
        //무게한테만 있다.
        // 다만 무기 데이터를 사용하는 컴포넌트는 플레이어가 참조한다.
    // 스킬 데이터도 스크립터블 오브젝트
        //스킬 한테만 있다.
        // 다만 스킬 데이터를 사용하는 컴포넌트는 플레이어가 참조한다.
    // 장비 데이터도 스크립터블 오브젝트
        // 장비한테만 있다 
        // 다만장비 데이터를 사용하는 컴포넌트는 플레이어가 참조한다.

    // 능력치가 변하는 타이밍은 언제일까?
        // 버프 스킬을 사용하거나 버프를 받았을때, 디버프를 받았을때,
        // 장비를 장착하고, 해제하는 타이밍


    // 효과는 다음과 같다.
        // 데미지를 입히는 효과
        // 버프를 받는 효과
        // UI를 변경하는 효과
    
    /*
        플레이어는 플레이어 데이터만 가지고 있나?
            무기의 컴포넌트를 제외하고는 맞는것 같다.
            무기는 무기 데이터만 가져야도 한다.
    */
}