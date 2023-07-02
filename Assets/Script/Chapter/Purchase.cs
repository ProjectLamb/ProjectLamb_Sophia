using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia_Carriers
{
    public class Purchase : Carrier
    {
        public override Carrier Clone()
        {
            if (this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
            Carrier res = Instantiate(this);
            res.DisableSelf();
            res.IsCloned = true;
            return res;
        }
        public override void Init(Entity _ownerEntity)
        {
            if (IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
            if (_ownerEntity == null) throw new System.Exception("투사체 생성 엔티티가 NULL임");
            IsInitialized = true;
        }
        public override void InitByObject(Entity _ownerEntity, object[] _objects)
        {
            Init(_ownerEntity);
        }
        public bool purchase(int price)
        {
            int current_gear = PlayerDataManager.GetPlayerData().Gear;
            Debug.Log(current_gear);

            if (current_gear >= price)  //구매 가능
            {
                PlayerDataManager.GetPlayerData().Gear -= price;
                return true;
            }
            else
                return false;
        }
    }
}

