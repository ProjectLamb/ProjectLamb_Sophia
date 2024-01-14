using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorFlocks : MonoBehaviour
{
    public Stage stage;
    public GameObject smallRaptor;
    public int smallAmount;
    private int index;
    private int mCurrentAmount;
    public int CurrentAmount
    {
        get
        {
            return mCurrentAmount;
        }
        set
        {
            mCurrentAmount = value;
            if (mCurrentAmount == 0)
            {
                stage.mobGenerator.CurrentMobCount--;
                Invoke("DestroySelf", 3f);
            }
        }
    }

    [SerializeField]
    private int mAttackCount;
    public int AttackCount
    {
        get
        {
            return mAttackCount;
        }
        set
        {
            mAttackCount = value;
            if (mAttackCount == HowlCount)
            {
                if (RaptorArray[0] != null)
                {
                    RaptorArray[0].GetComponent<Raptor>().DoHowl();
                }
            }
        }
    }
    int HowlCount;

    public GameObject[] RaptorArray;
    // Start is called before the first frame update
    void Awake()
    {
        smallAmount = Random.Range(1, 6);
        mCurrentAmount = 1;
        HowlCount = mCurrentAmount;
        RaptorArray = new GameObject[mCurrentAmount + smallAmount];
    }
    void Start()
    {
        RaptorArray[0] = transform.GetChild(0).gameObject;
        RaptorArray[0].GetComponent<Enemy>().stage = this.stage;
        //InstantiateSmallRaptor(smallAmount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void InstantiateSmallRaptor()
    {
        index = 1;
        for (int i = 0; i < smallAmount; i++)
        {
            GameObject temp;
            temp = Instantiate(smallRaptor, new Vector3(RaptorArray[0].transform.position.x + Random.Range(-5, 6), RaptorArray[0].transform.position.y, RaptorArray[0].transform.position.z + Random.Range(-5, 6)), Quaternion.identity);
            temp.GetComponent<Enemy>().stage = this.stage;
            RaptorArray[index++] = temp;
            temp.transform.parent = transform;
            mCurrentAmount++;
        }
    }

    public void Chase(bool flag)
    {
        for (int i = 0; i < index - 1; i++)
        {
            RaptorArray[i].GetComponent<Enemy>().isRecog = flag;
        }
    }
}