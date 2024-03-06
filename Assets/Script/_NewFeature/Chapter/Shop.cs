using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System;

namespace Sophia
{
    using Instantiates;
    public class Shop : MonoBehaviour
    {
        public GameObject equipmentPivot;
        public GameObject skillPivot;
        public GameObject heartPivot;
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
        private int skillCount = 0;
        public int SkillCount
        {
            get { return skillCount; }
            set { skillCount = value; }
        }
        private int equipmentPrice = 25;
        private int heartPrice = 10;
        private int skillPrice = 50;
        public ItemObject[] ItemArray;

        private UnityAction<int> instantiateItemEvent;

        void Awake()
        {
            ItemArray = new ItemObject[3];
            vendingMachine.GetComponent<VendingMachine>().price = 10;
            //rerollMachine.GetComponent<RerollMachine>().price = 5;
        }
        void Start()
        {
            GameManager.Instance.Shop = gameObject;

            instantiateItemEvent = (int _id) => { InstantiateItemById(_id); };
            InitShopItems();
        }

        public void InitShopItems()
        {
            for (int i = 0; i < 3; i++)
            {
                InstantiateOnDelayItemByFlag(i, 0);
            }
        }

        /// <summary>
        /// 아이템 생성을 하는데, 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flag"></param> <summary>
        public void InstantiateOnDelayItemByFlag(int id, int flag)
        {
            if (flag == 1)
            {
                AsyncWaitUse(2, id, instantiateItemEvent).Forget();
                return;
            }
            else
            {
                // InstantiateItemById(_id);
                instantiateItemEvent.Invoke(id);
            }
        }

        //public void instantiateItemEvent;
        private ItemObject GetRandomItem(string str) {
            System.Random random = new System.Random();
            ItemPool itemPoolRef = ItemPool.Instance;
            int equipmentCount = itemPoolRef._equipmentItems.Count;
            int skillCount = itemPoolRef._skillItems.Count;
            switch(str) {
                case "Equipment" :  {
                    var randIdx= random.Next(0, equipmentCount);
                    Debug.Log(randIdx);
                    return Instantiate(itemPoolRef._equipmentItems[randIdx]).Init();
                }
                case "Skill" :  {
                    var randIdx= random.Next(0, skillCount);
                    Debug.Log(randIdx);
                    return Instantiate(itemPoolRef._skillItems[randIdx]).Init();
                }
                case "Heart" :  {
                    return Instantiate(itemPoolRef._healthItem).Init();
                }
            }
            return null;
        }
        private void InstantiateItemById(int id)
        {
            ItemObject temp = null;
            ItemObjectBucket itemObjectBucket = null;
            if (id == 0 && equipmentCount < 3) //equipment
            {
                temp = GetRandomItem("Equipment");
                temp.gameObject.AddComponent<PurchaseComponent>();
                temp.GetComponent<PurchaseComponent>().price = equipmentPrice + equipmentCount * 5;

                temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.EquipmentCount++; };
                temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.InstantiateOnDelayItemByFlag(0, 1); };
                temp.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
                {
                    //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
                };

                itemObjectBucket = equipmentPivot.GetComponent<ItemObjectBucket>();
                ItemArray[0] = temp;
            }
            else if (id == 1)   //skill
            {
                temp = GetRandomItem("Skill");
                temp.gameObject.AddComponent<PurchaseComponent>();
                temp.GetComponent<PurchaseComponent>().price = skillPrice;

                temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.SkillCount++; };
                //temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => {this.InstantiateOnDelayItemByFlag(1, 1);};
                temp.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
                {
                    //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
                };

                itemObjectBucket = skillPivot.GetComponent<ItemObjectBucket>();
                ItemArray[1] = temp;
            }
            else if (id == 2)   //heart
            {
                temp = GetRandomItem("Heart");

                temp.gameObject.AddComponent<PurchaseComponent>();
                temp.GetComponent<PurchaseComponent>().price = heartPrice + heartCount * 5;

                temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.InstantiateOnDelayItemByFlag(2, 1); };
                temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.HeartCount++; };
                temp.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
                {
                    //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
                };

                itemObjectBucket = heartPivot.GetComponent<ItemObjectBucket>();
                ItemArray[2] = temp;
            }
            itemObjectBucket.InstantablePositioning(temp)
                            .SetTriggerTime(1f)
                            .Activate();
            temp.transform.parent = transform;
        }

        public async UniTaskVoid AsyncWaitUse(float _waitSecondTime, int _id, UnityAction<int> instantiateItemEvent)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
            instantiateItemEvent.Invoke(_id);
        }
    }
}