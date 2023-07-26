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
    private int equipmentPrice = 20;
    private int heartPrice = 10;
    private int skillPrice = 50;
    public Carrier[] ItemArray;

    private UnityAction<int> onPurchaseEvent;
    void Awake()
    {
        ItemArray = new Carrier[3];
        vendingMachine.GetComponent<VendingMachine>().price = 10;
        rerollMachine.GetComponent<RerollMachine>().price = 5;
    }
    void Start()
    {
        GameManager.Instance.Shop = gameObject;
        onPurchaseEvent = (int _id) => {InstantiateItemById(_id);};
        for (int i = 0; i < 3; i++)
        {
            InstantiateItem(i, 0);
        }
    }

    public void InstantiateItem(int id, int flag)
    {
        if (flag == 1)
        {
            AsyncWaitUse(2, id, onPurchaseEvent).Forget();
            return;
        }
        else  {
            onPurchaseEvent.Invoke(id);
        }
    }

    private void InstantiateItemById(int id){
        Carrier temp = null;
            CarrierBucket carrierBucket = null;
            if (id == 0 && equipmentCount < 3) //equipment
            {
                temp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Equipment").Clone();
                temp.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());
                temp.GetComponent<ItemEquipment>().IsShopItem = true;
                temp.GetComponent<ItemEquipment>().price = equipmentPrice + equipmentCount * 5;
                carrierBucket = equipmentPivot.GetComponent<CarrierBucket>();
                ItemArray[0] = temp;
            }
            else if (id == 1)   //skill
            {
                temp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Skill").Clone();
                temp.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());
                temp.GetComponent<ItemSkill>().IsShopItem = true;
                temp.GetComponent<ItemSkill>().price = skillPrice;
                carrierBucket = skillPivot.GetComponent<CarrierBucket>();
                ItemArray[1] = temp;
            }
            else if (id == 2)   //heart
            {
                temp = GameManager.Instance.GlobalCarrierManager.itemHeart.Clone();
                temp.InitByObject(null, new object[] { 30 });
                temp.GetComponent<ItemHeart>().IsShopItem = true;
                temp.GetComponent<ItemHeart>().price = heartPrice + heartCount * 5;
                carrierBucket = heartPivot.GetComponent<CarrierBucket>();
                ItemArray[2] = temp;
            }
            carrierBucket.CarrierTransformPositionning(gameObject, temp);
            temp.transform.parent = transform;
    }

    public async UniTaskVoid AsyncWaitUse(float _waitSecondTime, int _id, UnityAction<int> _onPurchaseEvent)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
        _onPurchaseEvent.Invoke(_id);
    }
}