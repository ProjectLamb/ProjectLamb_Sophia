using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia.Entitys
{
    public class MobGenerator : MonoBehaviour
    {
        Stage stage;

        public GameObject[] Mobs;
        public GameObject ElderOne;
        public List<GameObject> CurrentMobList;
        public int InitMobAmount = 3;

        [SerializeField]
        private List<float> spawnRateList;
        private int spawnAmount;

        void Awake()
        {
            CurrentMobList = new List<GameObject>();
        }

        public void InstantiateMob()
        {
            System.Random rand = new System.Random();
            while (spawnAmount > 0)
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
                instance.GetComponent<Enemy>()._objectiveEntity = GameManager.Instance.PlayerGameObject.GetComponent<Entity>();
                AddMob(instance);
                instance.transform.parent = transform.GetChild(4);

                switch (randomValue)
                {
                    default:
                        Sophia.Entitys.Enemy enemyTemp = instance.GetComponent<Sophia.Entitys.Enemy>();
                        enemyTemp.CurrentInstantiatedStage = stage;
                        break;
                }
                spawnAmount--;
            }
        }

        public void AddMob(GameObject mob)
        {
            CurrentMobList.Add(mob);
        }

        public void RemoveMob(GameObject mob)
        {
            CurrentMobList.Remove(mob);
        }

        public void InitMobGenerator()
        {
            System.Random rand = new System.Random();
            stage = GetComponent<Stage>();

            InitMobAmount *= stage.stageSizeRandom;
            spawnAmount = rand.Next(InitMobAmount, InitMobAmount + 3);
        }

        public int RandomMobPercent()
        {
            System.Random random = new System.Random();
            int returnValue = 0;
            float randomValue = (float)random.NextDouble() * 100.0f;

            float temp = 0.0f;

            for (int i = 0; i < Mobs.Length; i++)
            {
                temp += spawnRateList[i];

                if (randomValue <= temp)
                {
                    returnValue = i;
                    break;
                }
            }

            return returnValue;
        }
        public void SetMobsMovementOn()
        {
            foreach (GameObject enemy in CurrentMobList)
            {
                if (enemy.TryGetComponent<IMovable>(out IMovable mob))
                {
                    mob.SetMoveState(true);
                }
            }
        }

        public void SetMobsMovementOff()
        {
            foreach (GameObject enemy in CurrentMobList)
            {
                if (enemy.TryGetComponent<IMovable>(out IMovable mob))
                {
                    mob.SetMoveState(false);
                }
            }
        }
    }
}