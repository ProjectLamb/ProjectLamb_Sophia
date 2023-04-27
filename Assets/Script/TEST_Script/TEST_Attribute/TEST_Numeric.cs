public class TEST_Numeric {
        
        private int mMaxHP;
        
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

        
        private int mMaxStamina;
        
        private int mCurStamina;

        public int MaxStamina {get{return mMaxStamina;} set{mMaxStamina = value;}}
        public int CurStamina {
            get{return mCurStamina;} 
            set {
                if(mCurStamina < 0) mCurStamina = 0;
                mCurStamina = value;
            }
        }

        
        public float MoveSpeed {get; set;}

        
        public float Power {get; set;}

        
        private float mRange;
        public float Range {
            get {return mRange;} 
            set{
                if(mRange < 0) mRange = 0;
                mRange = value;
            }
        }

        
        float Luck {get; set;}

    }