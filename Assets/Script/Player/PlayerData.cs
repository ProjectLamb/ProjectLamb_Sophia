using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

/// <summary>
/// Numeric : int, float 같은것들만 저장 <br/>
/// * 최대체력, 현재체력, 최대스테미나, 현재스테미나, 이동속도, 힘, 사정거리(?), 운 <br/>
/// Attribute : bool, Dictionary, List 저장 <br/>
/// * 디버프상타, 버프상테, 시너지 상태 
/// </summary>
[ExecuteInEditMode]
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
                if(mMaxHP < CurHP) { CurHP = value;}
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
        public float Luck {get; set;}

        [field: SerializeField]
        public float Defense {get; set;}

        [field: SerializeField]
        public float Tenacity {get; set;}
    }
    public class Attribute{
        
        public int[] mDebuffState = new int[101];
        public int[] mBuffState = new int[101];
        public bool[] mSynergyState = new bool[101];
    }
    public Numeric numericData;
    public Attribute attributeData;
    public Weapon weapon; // 무기 클래스를 가져온다.

    [SerializedDictionary("Input Type", "Skill")]
    public SerializedDictionary<string, Skill> skills = new SerializedDictionary<string, Skill>(); // 무기 클래스를 가져온다.
    public List<Equipment> equipments = new List<Equipment>(8);

    [System.Serializable]
    public class Wealth {
        [field: SerializeField]
        public int Gear {get;set;}
        [field: SerializeField]
        public int Frag {get;set;}
    }
    public Wealth wealthData;

    [ContextMenu("Awake")]
    private void Awake() {
        if(weapon != null) weapon.playerData = this;
        if(skills.ContainsKey("Q")) skills["Q"].playerData = this;
    }

    public void FakePlayerDataContructor(){
        numericData = new PlayerData.Numeric();
        attributeData = new PlayerData.Attribute();
        wealthData = new Wealth();
    }
}