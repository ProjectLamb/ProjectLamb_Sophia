using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TEST
{
    public class ArthmeticActions : MonoBehaviour
    {
        UnityAction OneToFive;
        UnityAction TwoToFour;
        UnityAction OneToTwo;

        private void Awake()
        {
            OneToFive += func1;
            OneToFive += func2;
            OneToFive += func3;
            OneToFive += func4;
            OneToFive += func5;

            TwoToFour += func2;
            TwoToFour += func3;
            TwoToFour += func4;

            OneToTwo += func1;
            OneToTwo += func2;
        }

        [ContextMenu("OneToFive")]
        public void OTFInvoke()
        {
            OneToFive.Invoke();
        }

        [ContextMenu("TwoToFour")]
        public void TTFInvoke()
        {
            TwoToFour.Invoke();
        }

        [ContextMenu("OneToTwo")]
        public void OTTInvoke()
        {
            OneToTwo.Invoke();
        }

        [ContextMenu("OTF - OTT")]
        public void Arth1()
        {
            OneToFive -= OneToTwo;
        }


        public void func1()
        {
            Debug.Log("1");
        }
        public void func2()
        {
            Debug.Log("2");
        }
        public void func3()
        {
            Debug.Log("3");
        }
        public void func4()
        {
            Debug.Log("4");
        }
        public void func5()
        {
            Debug.Log("5");
        }
    }
}