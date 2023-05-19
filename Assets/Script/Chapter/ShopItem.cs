using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public bool IsEquipmentItem;
    public bool IsSkillItem;
    public bool IsHeartItem;
    [SerializeField]
    private int mItemPrice;
    public int ItemPrice
    {
        get { return mItemPrice; }
        set { mItemPrice = value; }
    }
    public int heartRecoveryRate;
    private GameObject shop;
    private string errorCode;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (IsEquipmentItem)  //부품
            {
                // if(부품 구매할 수 있는 조건)
                // {
                //     if(purchase(mItemPrice))
                //     {
                //         부품 획득 코드
                //         shop.GetComponent<Shop>().EquipmentCount++;
                //         Destroy(gameObject);
                //         return;
                //     }
                //     else
                //     {
                //         errorCode = "(기어 부족)";
                //     }
                // }
                // else
                // {
                //     errorCode = "(기어 못 사는 이유)";
                // }
            }
            else if (IsSkillItem)    //스킬
            {
                // if(스킬 카드 구매할 수 있는 조건)
                // {
                //     if(purchase(mItemPrice))
                //     {
                //         스킬 획득 코드
                //         Destroy(gameObject);
                //         return;
                //     }
                //     else
                //     {
                //         errorCode = "(기어 부족)";
                //     }
                // }
                // else
                // {
                //     errorCode = "(스킬 다 참)";
                // }
            }
            else if (IsHeartItem)    //체력
            {
                if (GameManager.Instance.playerGameObject.GetComponent<PlayerData>().numericData.CurHP < GameManager.Instance.playerGameObject.GetComponent<PlayerData>().numericData.MaxHP) //만약 플레이어 HP가 FULL HP가 아니고
                {
                    if (purchase(mItemPrice))
                    {
                        GameManager.Instance.playerGameObject.GetComponent<PlayerData>().numericData.CurHP += heartRecoveryRate;
                        shop.GetComponent<Shop>().HeartCount++;
                        Destroy(gameObject);
                        return;
                    }
                    else
                    {
                        errorCode = "(기어 부족)";
                    }
                }
                else
                {
                    errorCode = "(체력 다 참)";
                }
            }
            Debug.Log("아이템을 구매할 수 없습니다" + errorCode);   //UI로 출력할 것
        }
    }

    void Awake()
    {
        mItemPrice = 0;
        shop = transform.parent.gameObject;
    }
    public bool purchase(int price)
    {
        int current_gear = GameManager.Instance.playerGameObject.GetComponent<PlayerData>().wealthData.Gear;

        if (current_gear >= price)  //구매 가능
        {
            GameManager.Instance.playerGameObject.GetComponent<PlayerData>().wealthData.Gear -= price;
            return true;
        }
        else
            return false;
    }
}
