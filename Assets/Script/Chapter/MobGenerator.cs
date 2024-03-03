using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobGenerator : MonoBehaviour
{
    Stage stage;

    public GameObject[] Mobs;   //Enemy로 통일시키고 spawnRate 변수 추가
    public GameObject ElderOne;

    [SerializeField]
    public List<GameObject> mobList;
    public int InitMobAmount = 3;
    private int mobAmount;

    public void InstantiateMob()
    {
        System.Random rand = new System.Random();
        while (mobAmount > 0)
        {
            int i = rand.Next(1, stage.stageGenerator.width + 1);
            int j = rand.Next(1, stage.stageGenerator.height + 1);

            if (stage.stageGenerator.tileArray[i, j] == 0 || stage.stageGenerator.tileArray[i, j] == 2)
                continue;
            if (stage.stageGenerator.tileGameObjectArray[i, j].tag == "Portal")
                continue;

            int randomValue = RandomMobPercent();
            GameObject instance;
            instance = Instantiate(Mobs[randomValue], new Vector3(stage.stageGenerator.tileGameObjectArray[i, j].transform.position.x, transform.position.y, stage.stageGenerator.tileGameObjectArray[i, j].transform.position.z), Quaternion.identity);
            AddMob(instance);
            instance.transform.parent = transform.GetChild(4);

            switch (randomValue)
            {
                default:
                    Sophia.Entitys.Enemy enemyTemp = instance.GetComponent<Sophia.Entitys.Enemy>();
                    enemyTemp.CurrentInstantiatedStage = stage;
                    break;
            }
            mobAmount--;
        }
    }

    public void AddMob(GameObject mob)
    {
        mobList.Add(mob);
    }

    public void RemoveMob(GameObject mob)
    {
        mobList.Remove(mob);            
    }

    public void InitMobGenerator()
    {
        System.Random rand = new System.Random();
        stage = GetComponent<Stage>();

        InitMobAmount *= stage.stageSizeRandom;
        mobAmount = rand.Next(InitMobAmount, InitMobAmount + 3);
    }

    public int RandomMobPercent()
    {
        System.Random random = new System.Random();
        int returnValue = 0;
        float randomValue = (float)random.NextDouble() * 100.0f;

        float temp = 0.0f;

        for (int i = 0; i < Mobs.Length; i++)
        {
            Enemy enemy = Mobs[i].GetComponent<Enemy>();
            RaptorFlocks rf = Mobs[i].GetComponent<RaptorFlocks>();

            if(enemy != null)
                temp += Mobs[i].GetComponent<Enemy>().spawnRate;
            else
                temp += Mobs[i].GetComponent<RaptorFlocks>().spawnRate;
                
            if (randomValue <= temp)
            {
                returnValue = i;
                break;
            }
        }

        return returnValue;
    }
}
