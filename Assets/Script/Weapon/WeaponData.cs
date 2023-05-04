using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Numeric : int, float 같은것들만 저장 <br/>
/// * 데미지 비율, 무기 재사용 대기시간
/// Attribute : bool, Dictionary, List 저장 <br/>
/// * 디버프상타, 버프상테, 시너지 상태 
/// </summary>
public class WeaponData : MonoBehaviour
{

    [System.Serializable]
    public class Numeric{
        [field: SerializeField]
        public float DamageRatio {get; set;}

        [field: SerializeField]
		public float WeaponDelay {get;set;}
        
        [field: SerializeField]
        private float mRange;
        public float Range {
            get {return mRange;} 
            set{
                if(mRange < 0) mRange = 0;
                mRange = value;
            }
        }
        
    }
    public class Attribute {
        
    }

    public Numeric numericData;
    public Attribute attributeData;
}