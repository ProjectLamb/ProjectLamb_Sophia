using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorFlocks : MonoBehaviour
{
    public StageGenerator stageGenerator;
    public GameObject smallRaptor;
    int smallAmount;
    private int index = 1;
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
                stageGenerator.CurrentMobCount--;
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
            if(mAttackCount == HowlCount)
            {
                if(RaptorArray[0] != null)
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
        smallAmount = Random.Range(3, 6);
        mCurrentAmount = smallAmount + 1;
        HowlCount = mCurrentAmount;
        RaptorArray = new GameObject[mCurrentAmount];
    }
    void Start()
    {
        RaptorArray[0] = transform.GetChild(0).gameObject;
        RaptorArray[0].GetComponent<Enemy>().stageGenerator = this.stageGenerator;
        InstantiateSmallRaptor(smallAmount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void InstantiateSmallRaptor(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject temp;
            temp = Instantiate(smallRaptor, new Vector3(RaptorArray[0].transform.position.x + Random.Range(-5, 6), RaptorArray[0].transform.position.y, RaptorArray[0].transform.position.z + Random.Range(-5, 6)), Quaternion.identity);
            temp.GetComponent<Enemy>().stageGenerator = this.stageGenerator;
            RaptorArray[index++] = temp;
            temp.transform.parent = transform;
        }
    }

    public void Chase(bool flag)
    {
        for (int i = 0; i < index - 1; i++)
        {
            RaptorArray[i].GetComponent<Enemy>().chase = flag;
        }
    }
}
