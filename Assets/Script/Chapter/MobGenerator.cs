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
        while (amount > 0)
        {
            int i = Random.Range(1, stage.stageGenerator.width + 1);
            int j = Random.Range(1, stage.stageGenerator.height + 1);
            if (stage.stageGenerator.tileArray[i, j] == 0 || stage.stageGenerator.tileArray[i, j] == 2)
                continue;
            if (stage.stageGenerator.tileGameObjectArray[i, j].tag == "Portal")
                continue;
            int randomValue = Random.Range(0, Mobs.Length);
            GameObject instance;
            instance = Instantiate(Mobs[randomValue], new Vector3(stage.stageGenerator.tileGameObjectArray[i, j].transform.position.x, Mobs[randomValue].transform.localScale.y, stage.stageGenerator.tileGameObjectArray[i, j].transform.position.z), Quaternion.identity);
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
        stage = GetComponent<Stage>();
        mobCount = Random.Range(3, 3 + 3);
    }
}
