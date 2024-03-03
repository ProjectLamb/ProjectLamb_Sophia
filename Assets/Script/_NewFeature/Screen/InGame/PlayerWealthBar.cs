using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Sophia {
    public class PlayerWealthBar : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _gearWealthText; 
        [SerializeField] float _countSpeed = 1f;

        private int pervNumber = 0;
        private int countingNumber = 0;
        public int CountingNumber {
            get {return countingNumber;}
            set {countingNumber = value;}
        }

        public void SetPlayer(Entitys.Player player) {
            player.OnWealthChangeEvent += GearAddedHandler;
        }

        private void GearAddedHandler(int addingNum) {
            int destNum = countingNumber + addingNum;
            DOVirtual.Int(countingNumber, destNum, _countSpeed, (E) => {_gearWealthText.text = E.ToString();});
        }
    }
}