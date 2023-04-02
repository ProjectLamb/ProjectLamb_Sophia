using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyData : MonoBehaviour {
    [System.Serializable]
    public class Numeric {

        [field: SerializeField]
        private int mMaxHP;

        public int MaxHP {
            get{return mMaxHP;} 
            set{mMaxHP = value;}
        }
        [field: SerializeField]
        private int mCurHP;
        public int CurHP {
            get{return mCurHP;} 
            set{mCurHP = value;}
        }

        [field: SerializeField]
        public int MoveSpeed {get;set;}
    }

    public class Attribute {

    }

    public Numeric numericData;
    public Attribute attributeData;
}
