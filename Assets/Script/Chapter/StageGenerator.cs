using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using Sophia_Carriers;

public class StageGenerator : MonoBehaviour
{
    public enum STAGE_SIZE { SMALL = 1, MIDDLE, BIG, };
    //  StageGenerator //Stage[10];
    //  Stage[(int).Enum.Boss];
    //  Stage[(int).Enum.Boss];

    [Header("Stage Setting")]
    public int initWidth;
    public int initIncrease;

    //private화 하기
    public int width;
    public int height;
    private int increase;
    private int size;
    private int maxSize;
    private int stageSizeRandom;
    public int obstacleAmount;

    Stage stage;
    public GameObject[] portal;
    public GameObject tile;
    public GameObject[] WallSet;
    public GameObject transWall;
    public GameObject[] Props;
    public GameObject shop;
    public int[,] tileArray;    //0: empty, 1: tile, 2: wall
    public GameObject[,] tileGameObjectArray;
    public GameObject[] portalArray;

    [SerializeField]
    private bool mPortalE = false;
    public bool PortalE { get; set; }
    [SerializeField]
    private bool mPortalW = false;
    public bool PortalW { get; set; }
    [SerializeField]
    private bool mPortalS = false;
    public bool PortalS { get; set; }
    [SerializeField]
    private bool mPortalN = false;
    public bool PortalN { get; set; }
    int portalCount = 0;

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

    public void SetFloorType()
    {
        System.Random rand = new System.Random();
        int randomValue;
        int typeInterval = 0;
        switch (portalCount)    //포탈 개수
        {
            case 1: //1개
                randomValue = rand.Next(1, 4);
                break;
            case 2: //2개
                if (mPortalE && mPortalN || mPortalE && mPortalS ||
                mPortalW && mPortalN || mPortalW && mPortalS)   //ㄱ자 포탈
                {
                    randomValue = rand.Next(1, 4);
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
                    randomValue = rand.Next(1, 4);
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
                randomValue = rand.Next(1, 3);
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
                randomValue = rand.Next(1, 4);
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
    public void InstantiateTile()
    {
        float interval = tile.transform.localScale.x * 1.5f;   //tile interval
        float x = transform.position.x - interval * (width / 2);
        float z = transform.position.z - interval * (height / 2);
        for (int i = 1; i <= width; i++)
        {
            for (int j = 1; j <= height; j++)
            {
                GameObject instance = null;
                Vector3 pos = new Vector3(x + interval * i, 0, z + interval * j);
                if (tileArray[i, j] == 0)
                {
                    instance = Instantiate(transWall, pos, Quaternion.identity);
                    instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.TILE);
                }
                else if (tileArray[i, j] == 1)
                {
                    int randomValue = Random.Range(0, 4);
                    instance = Instantiate(tile, pos, Quaternion.Euler(0, 90 * randomValue, 0));
                    //instance = Instantiate(tile, pos, Quaternion.Euler(-90, 90 * randomValue, 0));
                    instance.GetComponent<Tile>().i = i;
                    instance.GetComponent<Tile>().j = j;
                    instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.TILE);
                    tileGameObjectArray[i, j] = instance;
                }
            }
        }
    }

    public void InstantiateWall()
    {
        GameObject instance = null;
        switch (stageSizeRandom)
        {
            case 1:
                instance = Instantiate(WallSet[(int)STAGE_SIZE.SMALL - 1], transform.position, Quaternion.identity);
                break;
            case 2:
                instance = Instantiate(WallSet[(int)STAGE_SIZE.MIDDLE - 1], transform.position, Quaternion.identity);
                break;
            case 3:
                instance = Instantiate(WallSet[(int)STAGE_SIZE.BIG - 1], transform.position, Quaternion.identity);
                break;
            default:
                instance = Instantiate(WallSet[(int)STAGE_SIZE.BIG - 1], transform.position, Quaternion.identity);
                break;
        }
        instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
    }
    public void InstantiatePortal()
    {
        GameObject portalModel = portal[(int)Stage.PORTAL_TYPE.NORMAL];
        GameObject instance;
        if(stage.Type == "boss")
        {
            //portalModel = portal[(int)Stage.PORTAL_TYPE.BOSS];
        }

        if (mPortalE)
        {
            instance = Instantiate(portalModel, tileGameObjectArray[1, (1 + height) / 2].transform.position, Quaternion.Euler(0, 90, 0));
            tileGameObjectArray[1, (1 + height) / 2].gameObject.tag = "Portal";
            instance.GetComponent<Portal>().PortalType = "east";
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.PORTAL);
            portalArray[0] = instance;
        }
        if (mPortalW)
        {
            instance = Instantiate(portalModel, tileGameObjectArray[width, (1 + height) / 2].transform.position, Quaternion.Euler(0, -90, 0));
            tileGameObjectArray[width, (1 + height) / 2].gameObject.tag = "Portal";
            instance.GetComponent<Portal>().PortalType = "west";
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.PORTAL);
            portalArray[1] = instance;
            transform.GetChild((int)Stage.STAGE_CHILD.WALL).GetChild(0).GetComponent<WallSet>().SetWestPortal();
        }
        if (mPortalN)
        {
            instance = Instantiate(portalModel, tileGameObjectArray[(1 + width) / 2, 1].transform.position, Quaternion.identity);
            tileGameObjectArray[(1 + width) / 2, 1].gameObject.tag = "Portal";
            instance.GetComponent<Portal>().PortalType = "north";
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.PORTAL);
            portalArray[2] = instance;
            transform.GetChild((int)Stage.STAGE_CHILD.WALL).GetChild(0).GetComponent<WallSet>().SetNorthPortal();
        }
        if (mPortalS)
        {
            instance = Instantiate(portalModel, tileGameObjectArray[(1 + width) / 2, height].transform.position, Quaternion.Euler(0, 180, 0));
            tileGameObjectArray[(1 + width) / 2, height].gameObject.tag = "Portal";
            instance.GetComponent<Portal>().PortalType = "south";
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.PORTAL);
            portalArray[3] = instance;
        }
    }

    public void InstantiateObstacle(int amount)
    {
        int temp = amount;
        while (temp > 0)
        {
            System.Random rand = new System.Random();
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
            int randomIndex = rand.Next(0, Props.Length);
            float randomDegree = (float)rand.NextDouble() * 360;
            instance = Instantiate(Props[randomIndex], tileGameObjectArray[i, j].transform.position, Quaternion.Euler(0, randomDegree, 0));
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.OBSTACLE);
            tileGameObjectArray[i, j].gameObject.tag = "Obstacle";
            temp--;
        }
    }

    public void InitStageGenerator(int random)
    {
        System.Random rand = new System.Random();
        width = initWidth;
        increase = initIncrease;
        stageSizeRandom = random;

        width += increase * stageSizeRandom;
        height = width;
        size = width * height;
        tileArray = new int[width + 1, height + 1];

        for (int i = 1; i <= width; i++)
            for (int j = 1; j <= height; j++)
                tileArray[i, j] = 1;

        tileGameObjectArray = new GameObject[width + 1, height + 1];

        switch (stageSizeRandom)
        {
            case 1:
                obstacleAmount = rand.Next(0, 6);
                break;
            case 2:
                obstacleAmount = rand.Next(0, 16);
                break;
            case 3:
                obstacleAmount = rand.Next(10, 26);
                break;
        }
    }
    public void GenerateNevMesh()
    {
        NavMeshSurface nav = GetComponent<NavMeshSurface>();
        nav.RemoveData();
        nav.BuildNavMesh();
    }

    private void Awake()
    {
        TryGetComponent<Stage>(out stage);
        obstacleAmount = 0;
        portalArray = new GameObject[4];
    }
}
