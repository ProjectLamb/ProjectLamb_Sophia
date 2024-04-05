using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System;
using Sophia_Carriers;
using Sophia.Instantiates;
using Sophia;
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
            //InstantiateItemById(id);
            instantiateItemEvent.Invoke(id);
        }
    }

    //public void instantiateItemEvent;

    private void InstantiateItemById(int id)
    {
        ItemObject item = null;
        ItemObjectBucket bucket = null;

        if (id == 0 && equipmentCount < 3) //equipment
        {
            // //temp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Equipment").Clone();
            // temp.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());

            // temp.gameObject.AddComponent<PurchaseComponent>();
            // temp.GetComponent<PurchaseComponent>().price = equipmentPrice + equipmentCount * 5;

            // temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.EquipmentCount++; };
            // temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.InstantiateOnDelayItemByFlag(0, 1); };
            // temp.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
            // {
            //     //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
            // };
            //bucket = equipmentPivot.GetComponent<CarrierBucket>();

            System.Random random = new System.Random();

            item = ItemPool.Instance._equipmentItems[random.Next(0, ItemPool.Instance._equipmentItems.Count)];
            bucket = equipmentPivot.GetComponent<ItemObjectBucket>();
            item = bucket.InstantablePositioning(Instantiate(item));
            ItemArray[0] = item;
            item.Init();
        }
        else if (id == 1)   //skill
        {
            // //temp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Skill").Clone();
            // item.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());
            // item.gameObject.AddComponent<PurchaseComponent>();
            // item.GetComponent<PurchaseComponent>().price = skillPrice;

            // item.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.SkillCount++; };
            // //temp.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => {this.InstantiateOnDelayItemByFlag(1, 1);};
            // item.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
            // {
            //     //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
            // };

            // bucket = skillPivot.GetComponent<CarrierBucket>();

            System.Random random = new System.Random();

            item = ItemPool.Instance._skillItems[random.Next(0, ItemPool.Instance._skillItems.Count)];
            bucket = skillPivot.GetComponent<ItemObjectBucket>();
            item = bucket.InstantablePositioning(Instantiate(item));
            ItemArray[1] = item;
            item.Init();
        }
        else if (id == 2)   //heart
        {
            // //temp = GameManager.Instance.GlobalCarrierManager.itemHeart.Clone();
            // item.InitByObject(null, new object[] { 30 });

            // item.gameObject.AddComponent<PurchaseComponent>();
            // item.GetComponent<PurchaseComponent>().price = heartPrice + heartCount * 5;

            // item.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.InstantiateOnDelayItemByFlag(2, 1); };
            // item.GetComponent<PurchaseComponent>().OnPurchasedEvent += () => { this.HeartCount++; };
            // item.GetComponent<PurchaseComponent>().OnPurchasedDenyEvent += () =>
            // {
            //     //GameManger.Instance.GlobalEvent.UI.PurchasedDeny();
            // };

            // bucket = heartPivot.GetComponent<CarrierBucket>();
            item = ItemPool.Instance._healthItem;
            bucket = heartPivot.GetComponent<ItemObjectBucket>();
            item = bucket.InstantablePositioning(Instantiate(item));
            ItemArray[2] = item;
            item.Init();
        }
        item.Activate();
        item.transform.parent = transform;
    }

    public async UniTaskVoid AsyncWaitUse(float _waitSecondTime, int _id, UnityAction<int> instantiateItemEvent)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
        instantiateItemEvent.Invoke(_id);
    }
}