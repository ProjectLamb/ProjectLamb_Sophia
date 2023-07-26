using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sophia_Carriers;

public class StageGenerator : MonoBehaviour
{
    int initWidth = 15;
    int initIncrease = 5;
    int width;
    int increase;
    int height;
    int wallHeight = 5;
    int size;
    int maxSize;
    [SerializeField]
    int stageSizeRandom;
    int obstacleAmount;
    [SerializeField]
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
    [SerializeField]
    private string mType;
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

    [SerializeField]
    private bool mIsClear;
    public bool IsClear
    {
        get { return mIsClear; }
        set { mIsClear = value; }
    }
    private bool mPortalE = false;
    private bool mPortalW = false;
    private bool mPortalS = false;
    private bool mPortalN = false;
    int portalCount = 0;
    float[] probs;
    float totalProbs = 100.0f;
    public GameObject portal;
    public GameObject tile;
    public GameObject wall;
    public GameObject transWall;
    public GameObject shop;
    int[,] tileArray;    //0: empty, 1: tile, 2: wall
    public GameObject[,] tileGameObjectArray;
    public GameObject[] Mobs;
    public GameObject ElderOne;
    public List<GameObject> mobArray;
    public GameObject[] portalArray;
    CarrierBucket carrierBucket;

    public void SetPortal(bool e, bool w, bool s, bool n)
    {
        if (e)
        {
            mPortalE = true;
            portalCount++;
        }
        if (w)
        {
            mPortalW = true;
            portalCount++;
        }
        if (s)
        {
            mPortalS = true;
            portalCount++;
        }
        if (n)
        {
            mPortalN = true;
            portalCount++;
        }
    }
    public int GetMaxSize()
    {
        maxSize = (initWidth + 3 * initIncrease) * (initWidth + 3 * initIncrease);
        return maxSize;
    }
    void SetFloorType()
    {
        int randomValue;
        int typeInterval = 0;
        switch (portalCount)    //포탈 개수
        {
            case 1: //1개
                randomValue = Random.Range(1, 4);
                break;
            case 2: //2개
                if (mPortalE && mPortalN || mPortalE && mPortalS ||
                mPortalW && mPortalN || mPortalW && mPortalS)   //ㄱ자 포탈
                {
                    randomValue = Random.Range(1, 4);
                    switch (randomValue)
                    {
                        case 1: //정사각형
                            break;
                        case 2: //작은 정사각형 꺾인 통로
                            typeInterval = 2;
                            if (mPortalE)
                            {
                                if (mPortalN)
                                {
                                    for (int i = 1; i <= width; i++)
                                    {
                                        for (int j = 1; j <= height; j++)
                                        {
                                            if ((width - (typeInterval + stageSizeRandom) <= i && i <= width) || (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                                tileArray[i, j] = 0;
                                        }
                                    }
                                }
                                else if (mPortalS)
                                {
                                    for (int i = 1; i <= width; i++)
                                    {
                                        for (int j = 1; j <= height; j++)
                                        {
                                            if ((width - (typeInterval + stageSizeRandom) <= i && i <= width) || (1 <= j && j <= 1 + (typeInterval + stageSizeRandom)))
                                                tileArray[i, j] = 0;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (mPortalN)
                                {
                                    for (int i = 1; i <= width; i++)
                                    {
                                        for (int j = 1; j <= height; j++)
                                        {
                                            if ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) || (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                                tileArray[i, j] = 0;
                                        }
                                    }
                                }
                                else if (mPortalS)
                                {
                                    for (int i = 1; i <= width; i++)
                                    {
                                        for (int j = 1; j <= height; j++)
                                        {
                                            if (1 <= i && i <= 1 + (typeInterval + stageSizeRandom) || (1 <= j && j <= 1 + (typeInterval + stageSizeRandom)))
                                                tileArray[i, j] = 0;
                                        }
                                    }
                                }
                            }
                            break;
                        case 3: //좁은 꺾인 통로
                            typeInterval = 2;
                            if (mPortalE)
                            {
                                if (mPortalN)
                                {
                                    for (int i = 1; i <= width; i++)
                                    {
                                        for (int j = 1; j <= height; j++)
                                        {
                                            if ((width - (typeInterval + stageSizeRandom) <= i && i <= width) || (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                                tileArray[i, j] = 0;
                                            if ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) && (1 <= j && j <= 1 + (typeInterval + stageSizeRandom)))
                                                tileArray[i, j] = 0;
                                        }
                                    }
                                }
                                else if (mPortalS)
                                {
                                    for (int i = 1; i <= width; i++)
                                    {
                                        for (int j = 1; j <= height; j++)
                                        {
                                            if ((width - (typeInterval + stageSizeRandom) <= i && i <= width) || (1 <= j && j <= 1 + (typeInterval + stageSizeRandom)))
                                                tileArray[i, j] = 0;
                                            if ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) && (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                                tileArray[i, j] = 0;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (mPortalN)
                                {
                                    for (int i = 1; i <= width; i++)
                                    {
                                        for (int j = 1; j <= height; j++)
                                        {
                                            if ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) || (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                                tileArray[i, j] = 0;
                                            if ((width - (typeInterval + stageSizeRandom) <= i && i <= width) && (1 <= j && j <= 1 + (typeInterval + stageSizeRandom)))
                                                tileArray[i, j] = 0;
                                        }
                                    }
                                }
                                else if (mPortalS)
                                {
                                    for (int i = 1; i <= width; i++)
                                    {
                                        for (int j = 1; j <= height; j++)
                                        {
                                            if (1 <= i && i <= 1 + (typeInterval + stageSizeRandom) || (1 <= j && j <= 1 + (typeInterval + stageSizeRandom)))
                                                tileArray[i, j] = 0;
                                            if ((width - (typeInterval + stageSizeRandom) <= i && i <= width) && (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                                tileArray[i, j] = 0;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                else    //1자
                {
                    randomValue = Random.Range(1, 4);
                    switch (randomValue)
                    {
                        case 1: //정사각형
                            break;
                        case 2: //일직선 복도형 통로
                            typeInterval = 2;
                            if (mPortalE)    //가로
                            {
                                for (int i = 1; i <= width; i++)
                                {
                                    for (int j = 1; j <= height; j++)
                                    {
                                        if ((1 <= j && j <= 1 + (typeInterval + stageSizeRandom)) ||
                                        (width - (typeInterval + stageSizeRandom) <= j && j <= width))
                                            tileArray[i, j] = 0;
                                    }
                                }
                            }
                            else    //세로
                            {
                                for (int i = 1; i <= width; i++)
                                {
                                    for (int j = 1; j <= height; j++)
                                    {
                                        if ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) ||
                                        (width - (typeInterval + stageSizeRandom) <= i && i <= width))
                                            tileArray[i, j] = 0;
                                    }
                                }
                            }
                            break;
                        case 3: //H형 통로
                            typeInterval = 2;
                            if (mPortalE)   //가로
                            {
                                for (int i = 1; i <= width; i++)
                                {
                                    for (int j = 1; j <= height; j++)
                                    {
                                        if ((width / 2 + 1 - (typeInterval + stageSizeRandom) <= i && i <= width / 2 + 1 + (typeInterval + stageSizeRandom)))
                                            if ((1 <= j && j <= 1 + (typeInterval + stageSizeRandom)) || (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                                tileArray[i, j] = 0;
                                    }
                                }
                            }
                            else    //세로
                            {
                                for (int i = 1; i <= width; i++)
                                {
                                    for (int j = 1; j <= height; j++)
                                    {
                                        if ((width / 2 + 1 - (typeInterval + stageSizeRandom) <= j && j <= width / 2 + 1 + (typeInterval + stageSizeRandom)))
                                            if ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) || (height - (typeInterval + stageSizeRandom) <= i && i <= height))
                                                tileArray[i, j] = 0;
                                    }
                                }
                            }
                            break;
                    }
                }
                break;
            case 3: //3개
                randomValue = Random.Range(1, 3);
                switch (randomValue)
                {
                    case 1: //정사각형
                        break;
                    case 2: //아치형 통로
                        typeInterval = 2;
                        if (!mPortalE)
                        {
                            for (int i = 1; i <= width; i++)
                            {
                                for (int j = 1; j <= height; j++)
                                {
                                    if (width - (typeInterval + stageSizeRandom) <= i && i <= width)
                                    {
                                        if ((1 <= j && j <= 1 + (typeInterval + stageSizeRandom)) || (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                            tileArray[i, j] = 0;
                                    }

                                    if (1 <= i && i <= 1 + (typeInterval + stageSizeRandom))
                                        tileArray[i, j] = 0;
                                }
                            }
                        }
                        else if (!mPortalW)
                        {
                            for (int i = 1; i <= width; i++)
                            {
                                for (int j = 1; j <= height; j++)
                                {
                                    if (1 <= i && i <= 1 + (typeInterval + stageSizeRandom))
                                    {
                                        if ((1 <= j && j <= 1 + (typeInterval + stageSizeRandom)) || (height - (typeInterval + stageSizeRandom) <= j && j <= height))
                                            tileArray[i, j] = 0;
                                    }

                                    if (width - (typeInterval + stageSizeRandom) <= i && i <= width)
                                        tileArray[i, j] = 0;
                                }
                            }
                        }
                        else if (!mPortalS)
                        {
                            for (int i = 1; i <= width; i++)
                            {
                                for (int j = 1; j <= height; j++)
                                {
                                    if (1 <= j && j <= 1 + (typeInterval + stageSizeRandom))
                                    {
                                        if ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) || (height - (typeInterval + stageSizeRandom) <= i && i <= height))
                                            tileArray[i, j] = 0;
                                    }

                                    if (width - (typeInterval + stageSizeRandom) <= j && j <= width)
                                        tileArray[i, j] = 0;
                                }
                            }
                        }
                        else if (!mPortalN)
                        {
                            for (int i = 1; i <= width; i++)
                            {
                                for (int j = 1; j <= height; j++)
                                {
                                    if (width - (typeInterval + stageSizeRandom) <= j && j <= width)
                                    {
                                        if ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) || (height - (typeInterval + stageSizeRandom) <= i && i <= height))
                                            tileArray[i, j] = 0;
                                    }

                                    if (1 <= j && j <= 1 + (typeInterval + stageSizeRandom))
                                        tileArray[i, j] = 0;
                                }
                            }

                        }
                        break;
                }
                break;
            case 4: //4개
                randomValue = Random.Range(1, 4);
                switch (randomValue)
                {
                    case 1: //정사각형
                        break;
                    case 2: //정사각형 중앙 막힘 통로
                        typeInterval = 1;
                        for (int i = 1; i <= width; i++)
                        {
                            for (int j = 1; j <= height; j++)
                            {
                                if ((width / 2 + 1 - (typeInterval + stageSizeRandom) <= i && i <= width / 2 + 1 + (typeInterval + stageSizeRandom)) &&
                                (height / 2 + 1 - (typeInterval + stageSizeRandom) <= j && j <= height / 2 + 1 + (typeInterval + stageSizeRandom)))
                                {
                                    tileArray[i, j] = 0;
                                }
                            }
                        }
                        break;
                    case 3: //십자가 형태 통로
                        typeInterval = 2;
                        for (int i = 1; i <= width; i++)
                        {
                            for (int j = 1; j <= height; j++)
                            {
                                if (((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) && (1 <= j && j <= 1 + (typeInterval + stageSizeRandom))) ||
                                ((width - (typeInterval + stageSizeRandom) <= i && i <= width) && (1 <= j && j <= 1 + (typeInterval + stageSizeRandom))) ||
                                ((1 <= i && i <= 1 + (typeInterval + stageSizeRandom)) && (height - (typeInterval + stageSizeRandom) <= j && j <= height)) ||
                                ((width - (typeInterval + stageSizeRandom) <= i && i <= width) && (height - (typeInterval + stageSizeRandom) <= j && j <= height)))
                                    tileArray[i, j] = 0;
                            }
                        }
                        break;
                }
                break;
        }
    }
    void InstantiateTile(int r, int c)
    {
        float interval = tile.transform.localScale.x * 1.5f;   //tile interval
        float x = transform.position.x - interval * (r / 2);
        float z = transform.position.z - interval * (c / 2);
        for (int i = 1; i <= r; i++)
        {
            for (int j = 1; j <= c; j++)
            {
                GameObject instance = null;
                Vector3 pos = new Vector3(x + interval * i, 0, z + interval * j);
                if (tileArray[i, j] == 0)
                {
                    instance = Instantiate(transWall, pos, Quaternion.identity);
                    instance.transform.parent = transform.GetChild(0);
                }
                else if (tileArray[i, j] == 1)
                {
                    int randomValue = Random.Range(0, 4);
                    instance = Instantiate(tile, pos, Quaternion.Euler(0, 90 * randomValue, 0));
                    //instance = Instantiate(tile, pos, Quaternion.Euler(-90, 90 * randomValue, 0));
                    instance.GetComponent<Tile>().i = i;
                    instance.GetComponent<Tile>().j = j;
                    instance.transform.parent = transform.GetChild(0);
                    // if (mType == "shop")
                    //     instance.GetComponent<Renderer>().material.color = Color.yellow;
                    // else if (mType == "start")
                    //     instance.GetComponent<Renderer>().material.color = Color.green;
                    // else if (mType == "boss")
                    //     instance.GetComponent<Renderer>().material.color = Color.red;
                    // else if (mType == "hidden")
                    //     instance.GetComponent<Renderer>().material.color = Color.black;
                    // else
                    //     instance.GetComponent<Renderer>().material.color = Color.grey;
                    tileGameObjectArray[i, j] = instance;
                }

                if (i == 1 || j == 1 || i == width || j == width)    //Instantiate Wall
                {
                    GameObject wallInstance;
                    Vector3 wallPos = pos;
                    if (j == 1)
                    {
                        for (int k = 0; k < wallHeight; k++)
                        {
                            wallPos = new Vector3(pos.x, (interval / 2) + interval * k, pos.z - interval);
                            wallInstance = Instantiate(wall, wallPos, Quaternion.identity);
                            wallInstance.transform.parent = transform.GetChild(1);
                        }
                    }
                    if (i == width)
                    {
                        for (int k = 0; k < wallHeight; k++)
                        {
                            wallPos = new Vector3(pos.x + interval, (interval / 2) + interval * k, pos.z);
                            wallInstance = Instantiate(wall, wallPos, Quaternion.identity);
                            wallInstance.transform.parent = transform.GetChild(1);
                        }
                    }
                    if (j == width)
                    {
                        wallPos = new Vector3(pos.x, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild(1);
                    }
                    if (i == 1)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild(1);
                    }

                    if (i == 1 && j == 1)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z - interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild(1);
                    }

                    if (i == 1 && j == width)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild(1);
                    }

                    if (i == width && j == 1)
                    {
                        wallPos = new Vector3(pos.x + interval, interval / 2, pos.z - interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild(1);
                    }

                    if (i == width && j == width)
                    {
                        wallPos = new Vector3(pos.x + interval, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild(1);
                    }
                }
            }
        }
    }

    void InstantiatePortal()
    {
        GameObject instance;
        if (mPortalE)
        {
            instance = Instantiate(portal, tileGameObjectArray[1, height / 2 + 1].transform.position, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Portal>().PortalType = "east";
            instance.transform.parent = transform.GetChild(2);
            portalArray[0] = instance;
        }
        if (mPortalW)
        {
            instance = Instantiate(portal, tileGameObjectArray[width, height / 2 + 1].transform.position, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Portal>().PortalType = "west";
            instance.transform.parent = transform.GetChild(2);
            portalArray[1] = instance;
        }
        if (mPortalN)
        {
            instance = Instantiate(portal, tileGameObjectArray[width / 2 + 1, 1].transform.position, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Portal>().PortalType = "north";
            instance.transform.parent = transform.GetChild(2);
            portalArray[2] = instance;
        }
        if (mPortalS)
        {
            instance = Instantiate(portal, tileGameObjectArray[width / 2 + 1, height].transform.position, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Portal>().PortalType = "south";
            instance.transform.parent = transform.GetChild(2);
            portalArray[3] = instance;
        }
    }

    void InstantiateObstacle(int amount)
    {
        int temp = amount;
        while (temp > 0)
        {
            int i = Random.Range(1, width + 1);
            int j = Random.Range(1, height + 1);
            if (tileArray[i, j] == 0 || tileArray[i, j] == 2)
                continue;
            if (mPortalN && (width / 2 - 1 <= i && i <= width / 2 + 2) && (1 <= j && j <= 3))   //포탈 주변 반경일 경우
                continue;
            if (mPortalS && (width / 2 - 1 <= i && i <= width / 2 + 2) && (height - 2 <= j && j <= height))
                continue;
            if (mPortalE && (1 <= i && i <= 3) && (height / 2 - 1 <= j && j <= height / 2 + 2))
                continue;
            if (mPortalW && (width - 2 <= i && i <= width) && (height / 2 - 1 <= j && j <= height / 2 + 2))
                continue;
            if (i == width / 2 + 1 && j == height / 2 + 1)  //중앙일 경우 -> 클리어 보상 소환 위치
                continue;
            GameObject instance;
            instance = Instantiate(wall, tileGameObjectArray[i, j].transform.position, Quaternion.identity);
            instance.transform.parent = transform.GetChild(3);
            temp--;
        }
    }

    void InstantiateMob(int amount)
    {
        while (amount > 0)
        {
            int i = Random.Range(1, width + 1);
            int j = Random.Range(1, height + 1);
            if (tileArray[i, j] == 0 || tileArray[i, j] == 2)
                continue;
            if (tileGameObjectArray[i, j].tag == "Portal")
                continue;
            int randomValue = Random.Range(0, Mobs.Length);
            GameObject instance;
            instance = Instantiate(Mobs[randomValue], new Vector3(tileGameObjectArray[i, j].transform.position.x, Mobs[randomValue].transform.localScale.y, tileGameObjectArray[i, j].transform.position.z), Quaternion.identity);
            mobArray.Add(instance);
            instance.transform.parent = transform.GetChild(4);

            switch (randomValue)
            {
                case (int)Enemy_TYPE.Enemy_Template:
                    instance.GetComponent<Enemy>().stageGenerator = this;
                    break;
                case (int)Enemy_TYPE.Raptor:
                    instance.GetComponent<RaptorFlocks>().stageGenerator = this;
                    break;
            }
            amount--;
        }
    }

    public void SetOnStage()
    {
        if (!mIsClear)
            GameManager.Instance.PlayerGameObject.GetComponent<Player>().IsPortal = false;
        GameManager.Instance.PlayerGameObject.GetComponent<Player>().IsPortal = false;
        GameManager.Instance.CurrentStage = this.gameObject;
        foreach (var m in mobArray)
        {
            if (m != null)
            {
                if (m.tag != "Enemy")
                {
                    m.GetComponent<RaptorFlocks>().Chase(true);
                }
                else
                    m.GetComponent<Enemy>().chase = true;
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetOffStage()
    {
        foreach (var m in mobArray)
        {
            if (m != null)
            {
                if (m.tag != "Enemy")
                {
                    m.GetComponent<RaptorFlocks>().Chase(false);
                }
                else
                    m.GetComponent<Enemy>().chase = false;
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void GenerateNevMesh()
    {
        NavMeshSurface nav = GetComponent<NavMeshSurface>();
        nav.RemoveData();
        nav.BuildNavMesh();
    }

    int Gacha()
    {
        int returnValue = 0;
        float randomValue = Random.value * totalProbs;

        float temp = 0.0f;

        for (int i = 0; i < 7; i++)
        {
            temp += probs[i];
            if (randomValue <= temp)
            {
                returnValue = i;
                break;
            }
        }

        return returnValue;
    }

    void StageClear()
    {
        mIsClear = true;
        Carrier tmp = null;
        if (Type == "hidden")
        {
            tmp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Skill").Clone();
            tmp.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());
        }
        else if (Type == "boss")
        {
            //다량의 기어, 부품 1종, 스킬 1종
        }
        else
        {
            switch (Gacha())
            {
                case 0:
                    Debug.Log("부품");
                    tmp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Equipment").Clone();
                    tmp.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());
                    break;
                case 1:
                    Debug.Log("체력");
                    tmp = GameManager.Instance.GlobalCarrierManager.itemHeart.Clone();
                    tmp.InitByObject(null, new object[] { 30 });
                    break;
                case 2:
                    Debug.Log("30원");
                    tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.DIAMOND].Clone();
                    tmp.Init(null);
                    break;
                case 3:
                    Debug.Log("20원");
                    tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.PLATINUM].Clone();
                    tmp.Init(null);
                    break;
                case 4:
                    Debug.Log("10원");
                    tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.GOLD].Clone();
                    tmp.Init(null);
                    break;
                case 5:
                    Debug.Log("5원");
                    tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.SILVER].Clone();
                    tmp.Init(null);
                    break;
                case 6:
                    Debug.Log("1원");
                    tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.BRONZE].Clone();
                    tmp.Init(null);
                    break;
            }
        }
        if (tileArray[width / 2 + 1, height / 2 + 1] == 1)
            carrierBucket.CarrierTransformPositionning(tileGameObjectArray[width / 2 + 1, height / 2 + 1].gameObject, tmp);
    }

    void Awake()
    {
        obstacleAmount = 0;
        mobCount = Random.Range(3, 3 + 3);
        mIsClear = false;
        portalArray = new GameObject[4];
        probs = new float[7] { 1.0f, 14.0f, 10.0f, 15.0f, 20.0f, 20.0f, 20.0f };
        carrierBucket = GetComponent<CarrierBucket>();
    }
    void Start()
    {
        width = initWidth;
        increase = initIncrease;
        if (mType == "normal")
        {
            stageSizeRandom = Random.Range(1, 4);   //1, 2, 3
        }
        else
            stageSizeRandom = 1;

        width += increase * stageSizeRandom;
        height = width;
        tileArray = new int[width + 1, height + 1];

        for (int i = 1; i <= width; i++)
            for (int j = 1; j <= height; j++)
                tileArray[i, j] = 1;

        if (mType == "normal")
        {
            SetFloorType();
        }

        tileGameObjectArray = new GameObject[width + 1, height + 1];
        InstantiateTile(width, height);
        InstantiatePortal();
        size = width * height;
        GenerateNevMesh();
        switch (stageSizeRandom)    //장애물 개수
        {
            case 1:
                obstacleAmount = Random.Range(0, 11);
                break;
            case 2:
                obstacleAmount = Random.Range(0, 21);
                break;
            case 3:
                obstacleAmount = Random.Range(10, 31);
                break;
        }
        if (mType == "normal")
        {
            InstantiateObstacle(obstacleAmount);
            InstantiateMob(mobCount);
        }
        else if (mType == "shop")
        {
            float x = 0;
            float y = 0;
            float z = 0;
            GameObject instance;
            if (mPortalE)
            {
                y = 180f;
            }
            else if (mPortalW)
            {

            }
            else if (mPortalN)
            {
                y = 90f;
            }
            else if (mPortalS)
            {
                y = 270f;
            }
            instance = Instantiate(shop, transform.position, Quaternion.Euler(x, y, z));
            instance.transform.parent = transform;
            mIsClear = true;
        }
        else if (mType == "hidden")
        {

        }
        else if (mType == "boss")
        {
            Instantiate(ElderOne, transform.position, Quaternion.identity);
        }
        CurrentMobCount = mobArray.Count;
        if (mType == "start")
        {
            GameObject character = GameManager.Instance.PlayerGameObject;
            GameManager.Instance.CurrentStage = this.gameObject;
            mIsClear = true;
            character.transform.position = new Vector3(transform.position.x, character.transform.position.y, transform.position.z);
        }
        else
        {
            SetOffStage();
        }
        transform.parent.GetComponent<ChapterGenerator>().LoadingStage++;   //로딩 완료
    }
    void Update()
    {
        if (!mIsClear)
        {
            if (CurrentMobCount == 0 && GameManager.Instance.CurrentStage == this.gameObject)
            {
                StageClear();
            }
        }
    }
}
