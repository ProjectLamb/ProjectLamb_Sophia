using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia_Carriers
{
    public class Shop : MonoBehaviour
    {
        public GameObject equipmentItem;
        public GameObject skillItem;
        public GameObject heartItem;
        public GameObject rerollMachine;
        public GameObject vendingMachine;
        private int equipmentCount = 0;
        public int EquipmentCount
        {
            get { return equipmentCount; }
            set { equipmentCount = value; }
        }
        private int heartCount = 0;
        public int HeartCount
        {
            get { return heartCount; }
            set { heartCount = value; }
        }
        private int equipmentPrice = 20;
        private int heartPrice = 10;
        private int skillPrice = 50;
        public GameObject[] rerollable;
        GameObject equipmentItemLocation;
        GameObject skillItemLocation;
        GameObject heartItemLocation;

        void Awake()
        {
            rerollable = new GameObject[2];
            rerollable[0] = equipmentItem;
            rerollable[1] = skillItem;
            equipmentItemLocation = transform.GetChild(0).gameObject;
            skillItemLocation = transform.GetChild(1).gameObject;
            heartItemLocation = transform.GetChild(2).gameObject;
        }
        void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                InstantiateItem(i);
            }
        }

        void Update()
        {
            //체력이랑 부품 리필되는 알고리즘
        }
        void InstantiateItem(int id)
        {
            GameObject temp = null;
            if (id == 0) //equipment
            {
                temp = Instantiate(equipmentItem, equipmentItemLocation.transform.position, Quaternion.identity);
                temp.GetComponent<ShopItem>().ItemPrice = equipmentPrice + equipmentCount * 5;
            }
            else if (id == 1)   //skill
            {
                temp = Instantiate(skillItem, skillItemLocation.transform.position, Quaternion.identity);
                temp.GetComponent<ShopItem>().ItemPrice = skillPrice;
            }
            else if (id == 2)   //heart
            {
                temp = Instantiate(heartItem, heartItemLocation.transform.position, Quaternion.identity);
                temp.GetComponent<ShopItem>().ItemPrice = heartPrice + heartCount * 5;
                temp.GetComponent<ShopItem>().heartRecoveryRate = 20;    //하트 회복 수치
            }
            temp.transform.parent = transform;
        }
    }
}