using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterGenerator : MonoBehaviour
{
    static int MAX = 15;
    public int startNumber = 1 + (MAX / 2) + (MAX / 2 * MAX);

    [SerializeField]
    int stageAmount;
    int minimumDistanceOfEndStage;
    [SerializeField]
    float hiddenStageSpawnRate;
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
        private bool mDiscovered;
        public bool Discovered
        {
            set
            {
                mDiscovered = value;
            }
            get{
                return mDiscovered;
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
        public string directionFromStart = null;
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
    };

    public Stage[] stage = new Stage[MAX * MAX + 1];

    void GenerateStage(int n)
    {
        int stageAmount = n;
        int maxStage = MAX * MAX;
        int amount; //현재 방 개수
        bool hiddenStage = Randomizer.GetThisChanceResult_Percentage(hiddenStageSpawnRate); //히든 보스

        // 큐 선언
        Queue<int> q = new Queue<int>();

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
            stage[startNumber].Type = "start";
            stage[startNumber].Vacancy = false;
            amount = 1;
            q.Enqueue(startNumber);

            int stageNum = Random.Range(2, 5);  //시작방 최소 인접 2 이상
            int directionNum = -1;
            while (stageNum > 0)
            {
                int num = startNumber;
                directionNum++;
                if (Randomizer.random())
                    continue;
                switch (directionNum % 4)
                {
                    case 0:
                        num--;
                        stage[startNumber].East = stage[num];
                        break;
                    case 1:
                        num++;
                        stage[startNumber].West = stage[num];
                        break;
                    case 2:
                        num -= MAX;
                        stage[startNumber].North = stage[num];
                        break;
                    case 3:
                        num += MAX;
                        stage[startNumber].South = stage[num];
                        break;
                }
                stage[num].Vacancy = false;
                stage[num].Depth = stage[q.Peek()].Depth + 1;
                q.Enqueue(num);
                stageNum--;
                amount++;
            }
        }

        // StageType 초기화
        Initialize();

        while (amount != stageAmount)   //BFS
        {
            if (q.Count == 0)
            {
                Initialize();
                q.Enqueue(stage[startNumber].StageNumber);
            }
            for (int i = 0; i < 4; i++) //Search 4 directions from the queue's front stage
            {
                int num = q.Peek(); //front stage number

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
                if (!stage[num].Vacancy)    //If the stage is already occupied
                    continue;
                if (stage[num].Type == "boundary")  //To avoid array boundary exception
                    continue;
                if (amount == stageAmount)  //If the stage amount is already full
                    continue;
                if (stage[num].CheckAdjacency(stage))   //To avoid circulating stage array
                    continue;
                if (Randomizer.random())    //50% chance to pass
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

        List<int> endL = new List<int>();
        List<int> bossL = new List<int>();
        List<int> hiddenL = new List<int>();
        string bossDirection = "";

        void DFS(Stage _currentStage, string _directionFromStart)
        {
            int count = 0;
            if (_currentStage.East != null)
            {
                if (_currentStage.Depth == 0)
                    _directionFromStart = "East";
                DFS(_currentStage.East, _directionFromStart);
                count++;
            }
            if (_currentStage.West != null)
            {
                if (_currentStage.Depth == 0)
                    _directionFromStart = "West";
                DFS(_currentStage.West, _directionFromStart);
                count++;
            }
            if (_currentStage.North != null)
            {
                if (_currentStage.Depth == 0)
                    _directionFromStart = "North";
                DFS(_currentStage.North, _directionFromStart);
                count++;
            }
            if (_currentStage.South != null)
            {
                if (_currentStage.Depth == 0)
                    _directionFromStart = "South";
                DFS(_currentStage.South, _directionFromStart);
                count++;
            }
            for (int i = 0; i < 4; i++)
            {
                int num = _currentStage.StageNumber;
                switch (i)
                {
                    case 0:
                        num--;
                        if (!stage[num].Vacancy)
                            _currentStage.East = stage[num];
                        break;
                    case 1:
                        num++;
                        if (!stage[num].Vacancy)
                            _currentStage.West = stage[num];
                        break;
                    case 2:
                        num -= MAX;
                        if (!stage[num].Vacancy)
                            _currentStage.North = stage[num];
                        break;
                    case 3:
                        num += MAX;
                        if (!stage[num].Vacancy)
                            _currentStage.South = stage[num];
                        break;
                }
            }
            if (count == 0)  //endStage
            {
                _currentStage.directionFromStart = _directionFromStart;
                if (_currentStage.Depth == Stage.maxDepth)
                    bossL.Add(_currentStage.StageNumber);
                else if (_currentStage.Depth >= minimumDistanceOfEndStage)
                    endL.Add(_currentStage.StageNumber);
            }
        }

        DFS(stage[startNumber], "");

        if (hiddenStage)
        {
            if (bossL.Count < 2 || bossL.Count + endL.Count < 3)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (bossL.Count + endL.Count < 2)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        int bossRandom = Random.Range(0, bossL.Count);
        stage[bossL[bossRandom]].Type = "boss";
        bossDirection = stage[bossL[bossRandom]].directionFromStart;
        bossL.RemoveAt(bossRandom);

        if (hiddenStage)
        {
            bool assign = false;

            foreach (int num in bossL)
            {
                endL.Add(num);
                if (stage[num].directionFromStart != bossDirection)
                {
                    hiddenL.Add(num);
                    assign = true;
                }
            }

            if (!assign)
            {
                foreach (int num in endL)
                {
                    if (stage[num].directionFromStart != bossDirection)
                    {
                        hiddenL.Add(num);
                        break;
                    }
                }
            }
            if (hiddenL.Count < 1)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            int hiddenRandom = Random.Range(0, hiddenL.Count);
            stage[hiddenL[hiddenRandom]].Type = "hidden";

            for (int i = 0; i < endL.Count; i++)
            {
                if (endL[i] != hiddenL[hiddenRandom])
                    continue;
                endL.RemoveAt(i);
            }
        }
        else
        {
            foreach (int num in bossL)
            {
                endL.Add(num);
            }
        }

        bossL.Clear();
        int endRandom = Random.Range(0, endL.Count);
        stage[endL[endRandom]].Type = "shop";

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
                instance.GetComponent<StageGenerator>().Type = stage[i].Type;
                instance.GetComponent<StageGenerator>().StageNumber = stage[i].StageNumber;
                instance.transform.parent = transform;

                if (stage[i].East != null)
                    portal[0] = true;
                if (stage[i].West != null)
                    portal[1] = true;
                if (stage[i].South != null)
                    portal[2] = true;
                if (stage[i].North != null)
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
        stageAmount = Random.Range(10, 16);
        minimumDistanceOfEndStage = 2;
        hiddenStageSpawnRate = 10f; //10% possibility
        GenerateStage(stageAmount);
    }
}