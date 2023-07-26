using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGenerator : MonoBehaviour
{
    public GameObject mob;
    public GameObject ElderOne;
    public List<GameObject> mobArray;
    int mobCount;
    private int mCurrentMobCount;
    public int CurrentMobCount
    {
        set
        {
            mCurrentMobCount = value;
        }
        get
        {
            return mCurrentMobCount;
        }
    }
    // public void InstantiateMob(int amount)
    // {
    //     while (amount > 0)
    //     {
    //         int i = Random.Range(1, width + 1);
    //         int j = Random.Range(1, height + 1);
    //         if (tileArray[i, j] == 0 || tileArray[i, j] == 2)
    //             continue;
    //         if (tileGameObjectArray[i, j].tag == "Portal")
    //             continue;
    //         GameObject instance;
    //         instance = Instantiate(mob, tileGameObjectArray[i, j].transform.position, Quaternion.identity);
    //         mobArray.Add(instance);
    //         instance.transform.parent = transform.GetChild(4);
    //         amount--;
    //     }
    // }
    void Awake()
    {
        mobCount = Random.Range(3, 3 + 3);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
