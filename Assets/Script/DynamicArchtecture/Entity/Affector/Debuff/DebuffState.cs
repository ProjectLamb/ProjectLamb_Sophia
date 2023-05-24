using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DebuffState {
    public List<IEnumerator> AsyncAffectorCoroutine;
    public List<UnityAction> Affector;
    public MasterData addingData; 
        //사실 캐릭터의 버프 상태가 WeaponData, SkillData, Player데이터를 동적조절하지 않는게 맞다.
        //그 이유는 오직 엔티티에 적용되는것이기 떄문이지.
        // 생각:저향력 수치를 어똏게 해야할까 생각이 든다. 지금 당장의 솔루션은 애초에 만들떄부터 FinalData를 EntiyData로 넘겨주는게 맞는듯하다. 
        // 참고: 아 Addition Data의 역할을 다시 깨닳았다. 바로 Entity의 수치를 변경하기 떄문이다.
        // 생각: 엔티티데이터가 Readonly인지 아닌지를 생각해야 한다.
        //  ㄴ 생각해봤다. 역시 Data는 전부 Final로 관리하는게 맞다. 접근하더라도 계산된놈으로 접근하는게 맞다는거야.
        //  그럼 Base녀석과 Get을할떄 가져올놈을 결정하는게 맞다는것.
            // Player같은 경우
                // Equipment는 Equip, Unequip DataChange 이벤트를 날려야되겠구나.;
                // PlayerData basePlayerData; PlayerData playerData;    
                // WeaponData baseWeaponData; WeaponData waeponData;
    public EntityData entityData;
}