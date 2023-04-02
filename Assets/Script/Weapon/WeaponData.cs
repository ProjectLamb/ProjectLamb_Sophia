using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    [System.Serializable]
    public class Numeric{
        [field: SerializeField]
        public float DamageRatio {get; set;}

        [field: SerializeField]
		public float WeaponDelay {get;set;}
    }
    public class Attribute {

    }

    public Numeric numericData;
    public Attribute attributeData;
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
	string WeaponType = "근거리, 원거리, 해킹";

	public class NumericData {
		float 공격력계수;
		float 공격딜레이;
		float 넉백수치;
	}

	public class AttributeData {
		//디버프 { 화상, 중독, 출혈, 수축,둔화, 혼란, 공포, 기절, 속박 };
		//버프 { n단공격 늘치명타 원소속성 캔슬가능여부 };
	}
}
*/