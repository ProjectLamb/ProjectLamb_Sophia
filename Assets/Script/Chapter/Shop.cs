using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System;
using Sophia_Carriers;
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
    public Carrier[] ItemArray;

    private UnityAction<int> instantiateItemEvent;

    void Awake()
    {
        ItemArray = new Carrier[3];
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

    private void InstantiateItemById(int id)
    {
        Carrier temp = null;
        CarrierBucket carrierBucket = null;
        if (id == 0 && equipmentCount < 3) //equipment
        {
            //temp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Equipment").Clone();
            temp.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());

            temp.gameObject.AddComponent<PurchaseComponent>();
            temp.GetComponent<PurchaseComponent>().price = equipmentPrice + equipmentCount * 5;

            temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.EquipmentCount++; };
            temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.InstantiateOnDelayItemByFlag(0, 1); };
            temp.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
            {
                //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
            };

            carrierBucket = equipmentPivot.GetComponent<CarrierBucket>();
            ItemArray[0] = temp;
        }
        else if (id == 1)   //skill
        {
            //temp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Skill").Clone();
            temp.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());
            temp.gameObject.AddComponent<PurchaseComponent>();
            temp.GetComponent<PurchaseComponent>().price = skillPrice;

            temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.SkillCount++; };
            //temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => {this.InstantiateOnDelayItemByFlag(1, 1);};
            temp.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
            {
                //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
            };

            carrierBucket = skillPivot.GetComponent<CarrierBucket>();
            ItemArray[1] = temp;
        }
        else if (id == 2)   //heart
        {
            //temp = GameManager.Instance.GlobalCarrierManager.itemHeart.Clone();
            temp.InitByObject(null, new object[] { 30 });

            temp.gameObject.AddComponent<PurchaseComponent>();
            temp.GetComponent<PurchaseComponent>().price = heartPrice + heartCount * 5;

            temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.InstantiateOnDelayItemByFlag(2, 1); };
            temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.HeartCount++; };
            temp.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
            {
                //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
            };

            carrierBucket = heartPivot.GetComponent<CarrierBucket>();
            ItemArray[2] = temp;
        }
        carrierBucket.CarrierTransformPositionning(gameObject, temp);
        temp.transform.parent = transform;
    }

    public async UniTaskVoid AsyncWaitUse(float _waitSecondTime, int _id, UnityAction<int> instantiateItemEvent)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
        instantiateItemEvent.Invoke(_id);
    }
}