using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGenerator : MonoBehaviour
{
    Stage stage;

    public GameObject[] Mobs;
    public GameObject ElderOne;
    public List<GameObject> mobArray;
    public int mobCount;
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

    public void InstantiateMob(int amount)
    {
        System.Random rand = new System.Random();
        while (amount > 0)
        {
            int i = rand.Next(1, stage.stageGenerator.width + 1);
            int j = rand.Next(1, stage.stageGenerator.height + 1);

            if (stage.stageGenerator.tileArray[i, j] == 0 || stage.stageGenerator.tileArray[i, j] == 2)
                continue;
            if (stage.stageGenerator.tileGameObjectArray[i, j].tag == "Portal")
                continue;

            int randomValue = rand.Next(0, Mobs.Length);
            GameObject instance;
            instance = Instantiate(Mobs[randomValue], new Vector3(stage.stageGenerator.tileGameObjectArray[i, j].transform.position.x, transform.position.y, stage.stageGenerator.tileGameObjectArray[i, j].transform.position.z), Quaternion.identity);
            mobArray.Add(instance);
            instance.transform.parent = transform.GetChild(4);

            switch (randomValue)
            {
                case (int)Enemy_TYPE.Enemy_Template:
                    instance.GetComponent<Enemy>().stage = stage;
                    break;
                case (int)Enemy_TYPE.Raptor:
                    instance.GetComponent<RaptorFlocks>().stage = stage;
                    break;
            }
            amount--;
        }
    }
    void Awake()
    {
        System.Random rand = new System.Random();
        stage = GetComponent<Stage>();
        mobCount = rand.Next(3, 3 + 3);
    }
}
