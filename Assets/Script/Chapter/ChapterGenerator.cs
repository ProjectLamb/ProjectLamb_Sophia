using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterGenerator : MonoBehaviour
{
    /**/
    int maxX = 15;
    int maxY = 15;

    [Tooltip("Amount is random from N to N + 2")]
    public int stageAmount; //editable or not
    public int minimumDistanceOfEndStage;
    public float hiddenStageSpawnRate;
    public List<GameObject> stageArray = new List<GameObject>();
    public GameObject stageObject;

    public struct Stage
    {
        public int GetStageNumber()
        {
            return stageNumber;
        }
        public void SetStageNumber(int n)
        {
            type = "normal";
            stageNumber = n;
        }
        public void SetStageType(string s)
        {
            type = s;
        }
        public string GetStageType()
        {
            return type;
        }
        public bool IsVacant()
        {
            return vacancy;
        }
        public void OccupyStage()
        {
            vacancy = false;
        }
        public void ResetStage()
        {
            vacancy = true;
        }
        public bool CheckAdjacency(Stage[] r, int maxX, int lower)
        {
            int i = 0;
            if (!r[stageNumber - 1].vacancy)
                i++;
            if (!r[stageNumber + 1].vacancy)
                i++;
            if (!r[stageNumber - maxX].vacancy)
                i++;
            if (!r[stageNumber + maxX].vacancy)
                i++;

            if (i >= lower)
                return true;
            else
                return false;
        }
        public bool CheckEndStage(Stage[] r, int maxX)
        {
            if (r[stageNumber].type == "start")
                return false;

            int i = 0;
            if (!r[stageNumber - 1].vacancy)
                i++;
            if (!r[stageNumber + 1].vacancy)
                i++;
            if (!r[stageNumber - maxX].vacancy)
                i++;
            if (!r[stageNumber + maxX].vacancy)
                i++;

            if (i == 1)
                return true;
            else
                return false;
        }

        public int CheckHiddenStage(Stage[] r, int maxX)
        {
            int stageNum = 0;

            if (CheckAdjacency(r, maxX, 3))
                stageNum = this.stageNumber;

            return stageNum;
        }
        public void SetDistanceFromStart(int init, int maxX)
        {
            int distance = 0;
            int initLine = init - (maxX / 2);
            int tmp = stageNumber;
            if (tmp < init)
            {
                while (tmp < initLine || tmp > initLine + maxX - 1)
                {
                    tmp += maxX;
                    distance++;
                }
                if (tmp < init)
                    distance += init - tmp;
                else
                    distance += tmp - init;
            }
            else
            {
                while (tmp < initLine || tmp > initLine + maxX - 1)
                {
                    tmp -= maxX;
                    distance++;
                }
                if (tmp < init)
                    distance += init - tmp;
                else
                    distance += tmp - init;
            }
            distanceFromStart = distance;
        }

        public int GetDistanceFromStart()
        {
            return distanceFromStart;
        }
        private int stageNumber;
        private bool vacancy;
        private int distanceFromStart;
        private string type;	//start, normal, shop, boss, boundary
    };

    public Stage[] stage;

    void GenerateStage(int n)
    {
        InfiniteLoopDetector.Run();

        stage = new Stage[maxX * maxY + 1];
        int stageAmount = n;
        int maxStage = maxX * maxY;

        // StageType 초기화      
        for (int i = 1; i <= maxStage; i++)
        {
            stage[i].SetStageNumber(i);
            if ((i >= 1 && i <= maxX) || i % maxX == 1 || i % maxX == 0 || (i >= (maxStage - maxX + 1) && i <= maxStage))
            {
                stage[i].SetStageType("boundary");
            }
        }

        /// 시작방 만들기
        int initStageNumber = 1 + (maxX / 2) + (maxY / 2 * maxX);
        stage[initStageNumber].SetStageType("start");

        // 큐 작업
        Queue<int> q = new Queue<int>();
        q.Enqueue(stage[initStageNumber].GetStageNumber());
        int amount = 0;

        while (amount != stageAmount)
        {
            if (q.Count == 0)
            {
                for (int i = 1; i <= maxStage; i++)
                    stage[i].ResetStage();
                q.Enqueue(stage[initStageNumber].GetStageNumber());
                stage[initStageNumber].SetStageType("start");
                amount = 0;
            }

            if (q.Peek() >= 1 && q.Peek() <= maxStage && stage[q.Peek()].IsVacant())  //Occupy stage
            {
                stage[q.Peek()].OccupyStage();
                amount++;
            }


            for (int i = 0; i < 4; i++)	//Search 4 directions from the queue's front stage
            {
                int num = q.Peek();	//front stage number

                switch (i)
                {
                    case 0: //Left
                        num--;
                        break;
                    case 1: //Right
                        num++;
                        break;
                    case 2: //Up
                        num -= maxX;
                        break;
                    case 3: //Down
                        num += maxX;
                        break;
                }

                if (stage[num].GetStageType() == "boundary")  //To avoid array boundary exception
                    continue;
                if (!stage[num].IsVacant())	//If the stage is already occupied
                    continue;
                if (stage[num].CheckAdjacency(stage, maxX, 2))	//To avoid circulating stage array
                    continue;
                if (amount == stageAmount)	//If the stage amount is already full
                    continue;
                if (Randomizer.random())	//50% chance to pass
                    continue;

                if (num >= 1 && num <= maxStage && stage[num].IsVacant())
                {
                    stage[num].OccupyStage();
                    amount++;
                }
                q.Enqueue(num);
            }
            q.Dequeue();
        }

        int farthest = 0;
        int farthestStage = -1;

        Queue<int> endQ = new Queue<int>();
        Queue<int> normalQ = new Queue<int>();

        for (int i = 1; i <= maxStage; i++)
            if (!stage[i].IsVacant())
            {
                stage[i].SetDistanceFromStart(initStageNumber, maxX);
                if (stage[i].CheckEndStage(stage, maxX) && stage[i].GetDistanceFromStart() >= minimumDistanceOfEndStage)
                    endQ.Enqueue(i);
                else
                    normalQ.Enqueue(i);
                if (stage[i].GetDistanceFromStart() > farthest)
                {
                    farthest = stage[i].GetDistanceFromStart();
                    farthestStage = i;
                }
            }
            else
            {
                if (stage[i].GetStageType() == "boundary")
                    continue;
                if (stage[i].CheckAdjacency(stage, maxX, 1))
                    stage[i].SetStageType("border");
            }

        for (int i = 0; i < endQ.Count; i++)
        {
            if (stage[endQ.Peek()].GetDistanceFromStart() == farthest)
                endQ.Dequeue();
            else
            {
                endQ.Enqueue(endQ.Peek());
                endQ.Dequeue();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        Queue<string> endStageSet = new Queue<string>();
        Queue<string> normalStageSet = new Queue<string>();
        //endStageSet.Enqueue("boss");
        endStageSet.Enqueue("shop");
        normalStageSet.Enqueue("middleboss");
        if (Randomizer.GetThisChanceResult_Percentage(hiddenStageSpawnRate))
        {
            Queue<int> hiddenQ = new Queue<int>();
            for (int i = 1; i <= maxStage; i++)
            {
                if (!stage[i].IsVacant())
                    continue;
                if (stage[i].GetStageType() != "border")
                    continue;
                int num = stage[i].CheckHiddenStage(stage, maxX);
                if (num != 0)
                    hiddenQ.Enqueue(num);
            }

            while (hiddenQ.Count != 0)
            {
                InfiniteLoopDetector.Run();
                hiddenQ.Enqueue(hiddenQ.Peek());
                hiddenQ.Dequeue();

                if (Randomizer.random())
                    continue;
                stage[hiddenQ.Peek()].OccupyStage();
                stage[hiddenQ.Peek()].SetStageType("hidden");
                break;
            }
            Debug.Log("Hidden Stage Spawned");
        }

        if (endQ.Count < endStageSet.Count || farthestStage == -1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        stage[farthestStage].SetStageType("boss");

        while (endStageSet.Count != 0)
        {
            InfiniteLoopDetector.Run();

            endQ.Enqueue(endQ.Peek());
            endQ.Dequeue();

            if (stage[endQ.Peek()].GetStageType() == "boss")
                continue;
            if (!Randomizer.random())
                continue;
            stage[endQ.Peek()].SetStageType(endStageSet.Peek());
            endQ.Dequeue();
            endStageSet.Dequeue();
        }

        while (normalStageSet.Count != 0)
        {
            InfiniteLoopDetector.Run();
            normalQ.Enqueue(normalQ.Peek());
            normalQ.Dequeue();

            if (stage[normalQ.Peek()].GetDistanceFromStart() < 2) //minimum distance of middle boss
                continue;
            if (!Randomizer.random())
                continue;
            stage[normalQ.Peek()].SetStageType(normalStageSet.Peek());
            normalQ.Dequeue();
            normalStageSet.Dequeue();
        }

        //Assign real stage objects in Unity

        int x = 0;
        int z = 0;
        int stageInterval = stageObject.GetComponent<StageGenerator>().GetMaxSize(); //stage gameobect's width

        for (int i = 1; i <= maxX * maxY; i++)
        {
            Vector3 stagePos = new Vector3(x, 0, z);
            if (!stage[i].IsVacant())
            {
                GameObject instance;
                bool[] portal = new bool[4]; //east, west, south, north
                instance = Instantiate(stageObject, stagePos, Quaternion.identity);
                instance.GetComponent<StageGenerator>().SetStageType(stage[i].GetStageType());
                instance.GetComponent<StageGenerator>().SetStageLocation(stagePos.x, stagePos.z);
                instance.transform.parent = transform;
                stageArray.Add(instance);

                if (!stage[i - 1].IsVacant())
                    portal[0] = true;
                if (!stage[i + 1].IsVacant())
                    portal[1] = true;
                if (!stage[i + maxX].IsVacant())
                    portal[2] = true;
                if (!stage[i - maxX].IsVacant())
                    portal[3] = true;
                instance.GetComponent<StageGenerator>().SetPortal(portal[0], portal[1], portal[2], portal[3]);
            }
            if (i % maxX == 0)
            {
                x = 0;
                z += stageInterval;
            }
            else
            {
                x += stageInterval;
            }
        }
    }
    void Awake()
    {
        stageAmount = Random.Range(stageAmount, stageAmount + 3);
        hiddenStageSpawnRate = 10f;
        Debug.Log("Generated amount of stages: " + stageAmount);
        GenerateStage(stageAmount);
    }
}