using UnityEngine;

namespace Sophia.Composite
{
    using Sophia.DataSystem.Modifiers.Affector;
    using Sophia.DataSystem.Modifiers.ConcreteAffectors;

    public struct DataForFunction {
        public int[] ints;
        public float[] floats;
    }

    class AffectorFactory {

#region Extras List
        
        long bitset;
        public DataForFunction[] datas = new DataForFunction[64];

        // Extras OnMoveExtras;
#endregion

#region Invoke

        public void foo() {
            for(int i = 0; i < 64; i++) {
                if(1 == (bitset & 1 << i)) { SomeFunction(i);  }
            }
        }

        // OnMoveExtras.Invoke();
#endregion

#region Modifiers
        public void AddFunction(int i) => bitset &= 1 << i;
        // OnMoveExtras.AddModifier();
        public void RemoveFunction(int i) => bitset ^= 1 << i;
        // OnMoveExtras.RemoveModifier();
#endregion
        /*
        */
        void SomeFunction(int i) {
            switch (i)
            {
                case 0  : {
                    int speed = datas[0].ints[0];
                    int damage = datas[0].ints[1];
                    /*...*/
                    break;
                }
                case 1  : { 
                    int speed = datas[1].ints[3];
                    break;  
                }
                case 2  : { break;  }
                case 3  : { break;  }
                case 4  : { break;  }
                case 5  : { break;  }
                case 6  : { break;  }
                case 7  : { break;  }
                case 8  : { break;  }
                case 9  : { break;  }
                case 10 : { break;  }
                case 11 : { break;  }
                case 12 : { break;  }
                case 13 : { break;  }
                case 14 : { break;  }
                case 15 : { break;  }
                case 16 : { break;  }
                case 17 : { break;  }
                case 18 : { break;  }
                case 19 : { break;  }
                case 20 : { break;  }
                case 21 : { break;  }
                case 22 : { break;  }
                default : { break;}
            }
        }
    }
}