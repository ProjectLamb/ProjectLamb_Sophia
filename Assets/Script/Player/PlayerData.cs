using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Numeric : int, float 같은것들만 저장 <br/>
/// * 최대체력, 현재체력, 최대스테미나, 현재스테미나, 이동속도, 힘, 사정거리(?), 운 <br/>
/// Attribute : bool, Dictionary, List 저장 <br/>
/// * 디버프상타, 버프상테, 시너지 상태 
/// </summary>
public class PlayerData : MonoBehaviour
{
    [System.Serializable]
    public class Numeric {
        [field: SerializeField]
        private int mMaxHP;
        [field: SerializeField]
        private int mCurHP;

        public int MaxHP {
            get{return mMaxHP;} 
            set{
                if(mMaxHP < 0) mMaxHP = 0;
                mMaxHP = value;
            }
        }

        public int CurHP {
            get {return mCurHP;}
            set {
                if(mCurHP < 0) mCurHP = 0;
                mCurHP = value;
            }
        }

        [field: SerializeField]
        private int mMaxStamina;
        [field: SerializeField]
        private int mCurStamina;

        public int MaxStamina {get{return mMaxStamina;} set{mMaxStamina = value;}}
        public int CurStamina {
            get{return mCurStamina;} 
            set {
                if(mCurStamina < 0) mCurStamina = 0;
                mCurStamina = value;
            }
        }

        [field: SerializeField]
        public float MoveSpeed {get; set;}

        [field: SerializeField]
        public float Power {get; set;}

        [field: SerializeField]
        private float mRange;
        public float Range {
            get {return mRange;} 
            set{
                if(mRange < 0) mRange = 0;
                mRange = value;
            }
        }

        [field: SerializeField]
        float Luck {get; set;}
    }
    public class Attribute{
        public Dictionary<E_DebuffState, bool> mDebuffState;        
        public Dictionary<E_BuffState, bool> mBuffState;
        public Dictionary<E_SynergyState, bool> mSynergyState;
    }
    public Numeric numericData;
    public Attribute attributeData;
}

/*
```cs
데커스데이터 {
    데커스수치 수치;
    데커스상태 상태;
}

데커스수치 {
    int 현재체력; int 최대체력;
    int 현재스테미나; int 최대스테미나;
    float 현재이동속도;

    float 공격력;
    float 사거리;
    float 공격속도;
    
    float 운; //치명타확률 회피율
}
*/