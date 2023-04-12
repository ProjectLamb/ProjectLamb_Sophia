using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterGenerator : MonoBehaviour
{
    /**/
    static int MAX = 15;

    [SerializeField]
    int stageAmount;
    int minimumDistanceOfEndStage;
    [SerializeField]
    float hiddenStageSpawnRate;
    public List<GameObject> stageArray = new List<GameObject>();
    public GameObject obj;

    public class Stage
    {
        //type
        private int mStageNumber;
        public int StageNumber
        {
            get
            {
                return mStageNumber;
            }
            set
            {
                mStageNumber = value;
            }
        }
        private bool mVacancy;
        public bool Vacancy
        {
            set
            {
                mVacancy = value;
            }
            get
            {
                return mVacancy;
            }
        }
        private int mDepth;
        public int Depth
        {
            get
            {
                return mDepth;
            }
            set
            {
                mDepth = value;
                if (mDepth > maxDepth)
                    maxDepth = mDepth;
            }
        }
        public static int maxDepth = -1;
        private string mType;	//start, normal, shop, boss, boundary, hidden
        public string Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }
        public Stage East = null;
        public Stage West = null;
        public Stage South = null;
        public Stage North = null;
        public GameObject stageObject = null;

        //methods
        public Stage()
        {
            mStageNumber = 0;
            mType = "normal";
            mVacancy = true;
            mDepth = 0;
        }
        public bool CheckAdjacency(Stage[] r)
        {
            int i = 0;
            if (!r[mStageNumber - 1].mVacancy)
                i++;
            if (!r[mStageNumber + 1].mVacancy)
                i++;
            if (!r[mStageNumber - MAX].mVacancy)
                i++;
            if (!r[mStageNumber + MAX].mVacancy)
                i++;

            if (i > 1)
                return true;
            else
                return false;
        }
        public bool CheckEndStage(Stage[] r)
        {
            if (r[mStageNumber].mType == "start")
                return false;

            int i = 0;
            if (!r[mStageNumber - 1].mVacancy)
                i++;
            if (!r[mStageNumber + 1].mVacancy)
                i++;
            if (!r[mStageNumber - MAX].mVacancy)
                i++;
            if (!r[mStageNumber + MAX].mVacancy)
                i++;

            if (i == 1)
                return true;
            else
                return false;
        }
    };

    public Stage[] stage = new Stage[MAX * MAX + 1];

    void GenerateStage(int n)
    {
        int stageAmount = n;
        int maxStage = MAX * MAX;
        int initStageNumber = 1 + (MAX / 2) + (MAX / 2 * MAX);
        int amount; //현재 방 개수

        void Initialize()
        {
            Stage.maxDepth = -1;
            for (int i = 1; i <= maxStage; i++)
            {
                stage[i] = new Stage();
                stage[i].StageNumber = i;
                if ((i >= 1 && i <= MAX) || i % MAX == 1 || i % MAX == 0 || (i >= (maxStage - MAX + 1) && i <= maxStage))
                {
                    stage[i].Type = "boundary";
                }
            }
            /// 시작방 만들기
            stage[initStageNumber].Type = "start";
            amount = 0;
        }

        // StageType 초기화
        Initialize();

        // 큐 작업
        Queue<int> q = new Queue<int>();
        q.Enqueue(stage[initStageNumber].StageNumber);

        while (amount != stageAmount)
        {
            if (q.Count == 0)
            {
                Initialize();
                q.Enqueue(stage[initStageNumber].StageNumber);
            }

            if (q.Peek() >= 1 && q.Peek() <= maxStage && stage[q.Peek()].Vacancy)  //Occupy stage
            {
                stage[q.Peek()].Vacancy = false;
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
                        num -= MAX;
                        break;
                    case 3: //Down
                        num += MAX;
                        break;
                }
                if (stage[num].Type == "boundary")  //To avoid array boundary exception
                    continue;
                if (!stage[num].Vacancy)	//If the stage is already occupied
                    continue;
                if (stage[num].CheckAdjacency(stage))	//To avoid circulating stage array
                    continue;
                if (amount == stageAmount)	//If the stage amount is already full
                    continue;
                if (Randomizer.random())	//50% chance to pass
                    continue;

                if (num >= 1 && num <= maxStage && stage[num].Vacancy)
                {
                    stage[num].Vacancy = false;
                    stage[num].Depth = stage[q.Peek()].Depth + 1;
                    switch (i)
                    {
                        case 0:
                            stage[q.Peek()].East = stage[num];
                            break;
                        case 1:
                            stage[q.Peek()].West = stage[num];
                            break;
                        case 2:
                            stage[q.Peek()].North = stage[num];
                            break;
                        case 3:
                            stage[q.Peek()].South = stage[num];
                            break;
                    }
                    amount++;
                }
                q.Enqueue(num);
            }
            q.Dequeue();
        }

        // int farthest = 0;
        int farthestStage = -1;

        Queue<int> endQ = new Queue<int>();

        for (int i = 1; i <= maxStage; i++)
            if (!stage[i].Vacancy)
            {
                if (stage[i].CheckEndStage(stage) && stage[i].Depth >= minimumDistanceOfEndStage)   //end Stage
                    endQ.Enqueue(i);

                if (stage[i].Depth == Stage.maxDepth)
                    farthestStage = i;
            }

        for (int i = 0; i < endQ.Count; i++)    //이미 배정된 보스 방은 큐에서 제거
        {
            if (endQ.Peek() == farthestStage)
                endQ.Dequeue();
            else
            {
                endQ.Enqueue(endQ.Peek());
                endQ.Dequeue();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        Queue<string> endStageSet = new Queue<string>();
        endStageSet.Enqueue("shop");
        if (Randomizer.GetThisChanceResult_Percentage(hiddenStageSpawnRate))
        {
            endStageSet.Enqueue("hidden");
            Debug.Log("Hidden Stage Spawned");
        }

        if (endQ.Count < endStageSet.Count || farthestStage == -1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        stage[farthestStage].Type = "boss"; //가장 먼 방은 보스방으로 배정

        while (endStageSet.Count != 0)
        {
            InfiniteLoopDetector.Run();

            endQ.Enqueue(endQ.Peek());
            endQ.Dequeue();

            if (stage[endQ.Peek()].Type == "boss")
                continue;
            if (!Randomizer.random())
                continue;
            stage[endQ.Peek()].Type = endStageSet.Peek();
            endQ.Dequeue();
            endStageSet.Dequeue();
        }
        //Assign real stage objects in Unity

        int x = 0;
        int z = 0;
        int stageInterval = obj.GetComponent<StageGenerator>().GetMaxSize(); //stage gameobect's width

        for (int i = 1; i <= maxStage; i++)
        {
            Vector3 stagePos = new Vector3(x, 0, z);
            if (!stage[i].Vacancy)
            {
                GameObject instance;
                bool[] portal = new bool[4]; //east, west, south, north
                instance = Instantiate(obj, stagePos, Quaternion.identity);
                instance.GetComponent<StageGenerator>().SetStageType(stage[i].Type);
                instance.GetComponent<StageGenerator>().SetStageLocation(stagePos.x, stagePos.z);
                instance.transform.parent = transform;
                stageArray.Add(instance);

                if (!stage[i - 1].Vacancy)
                    portal[0] = true;
                if (!stage[i + 1].Vacancy)
                    portal[1] = true;
                if (!stage[i + MAX].Vacancy)
                    portal[2] = true;
                if (!stage[i - MAX].Vacancy)
                    portal[3] = true;
                instance.GetComponent<StageGenerator>().SetPortal(portal[0], portal[1], portal[2], portal[3]);
                stage[i].stageObject = instance;
            }
            if (i % MAX == 0)
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
        stageAmount = 10;
        minimumDistanceOfEndStage = 3;
        hiddenStageSpawnRate = 50f; //10% possibility
        //Debug.Log("Generated amount of stages: " + stageAmount);
        GenerateStage(stageAmount);
    }
}