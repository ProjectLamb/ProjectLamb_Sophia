using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sophia_Carriers;

public class StageGenerator : MonoBehaviour
{
    public enum SIZE { SMALL = 1, MIDDLE, BIG, };
    //  StageGenerator //Stage[10];
    //  Stage[(int).Enum.Boss];
    //  Stage[(int).Enum.Boss];
    Stage stage;

    public int initWidth = 15;
    public int initIncrease = 5;
    public int width;
    public int increase;
    public int height;
    int wallHeight = 5;
    public int size;
    int maxSize;
    [SerializeField]
    public int stageSizeRandom;
    public int obstacleAmount;

    public GameObject[] portal;
    public GameObject tile;
    public GameObject[] WallSet;
    public GameObject transWall;
    public GameObject obstacle;
    public GameObject shop;
    public int[,] tileArray;    //0: empty, 1: tile, 2: wall
    public GameObject[,] tileGameObjectArray;
    public GameObject[] portalArray;

    private bool mPortalE = false;
    public bool PortalE { get; set; }
    private bool mPortalW = false;
    public bool PortalW { get; set; }
    private bool mPortalS = false;
    public bool PortalS { get; set; }
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
    public void InstantiateTile(int r, int c)
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

                /*if (i == 1 || j == 1 || i == width || j == width)    //Instantiate Wall
                {
                    GameObject wallInstance;
                    Vector3 wallPos = pos;
                    if (j == 1)
                    {
                        for (int k = 0; k < wallHeight; k++)
                        {
                            wallPos = new Vector3(pos.x, (interval / 2) + interval * k, pos.z - interval);
                            wallInstance = Instantiate(wall, wallPos, Quaternion.identity);
                            wallInstance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
                        }
                    }
                    if (i == width)
                    {
                        for (int k = 0; k < wallHeight; k++)
                        {
                            wallPos = new Vector3(pos.x + interval, (interval / 2) + interval * k, pos.z);
                            wallInstance = Instantiate(wall, wallPos, Quaternion.identity);
                            wallInstance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
                        }
                    }
                    if (j == width)
                    {
                        wallPos = new Vector3(pos.x, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
                    }
                    if (i == 1)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
                    }

                    if (i == 1 && j == 1)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z - interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
                    }

                    if (i == 1 && j == width)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
                    }

                    if (i == width && j == 1)
                    {
                        wallPos = new Vector3(pos.x + interval, interval / 2, pos.z - interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
                    }

                    if (i == width && j == width)
                    {
                        wallPos = new Vector3(pos.x + interval, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
                    }
                }*/
            }
        }
    }

    public void InstantiateWall()
    {
        GameObject instance = null;
        switch (stageSizeRandom)
        {
            case 1:
                instance = Instantiate(WallSet[(int)SIZE.SMALL - 1], transform.position, Quaternion.identity);
                break;
            case 2:
                instance = Instantiate(WallSet[(int)SIZE.MIDDLE - 1], transform.position + new Vector3(30, 0, -20), Quaternion.identity);
                break;
            case 3:
                instance = Instantiate(WallSet[(int)SIZE.BIG - 1], transform.position + new Vector3(49, 0, -49), Quaternion.identity);
                break;
            default:
                instance = Instantiate(WallSet[(int)SIZE.MIDDLE - 1], transform.position, Quaternion.identity);
                break;
        }
        instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.WALL);
    }
    public void InstantiatePortal()
    {
        GameObject instance;
        if (mPortalE)
        {
            instance = Instantiate(portal[(int)Stage.PORTAL_TYPE.NORMAL], tileGameObjectArray[1, height / 2].transform.position, Quaternion.Euler(0, 90, 0));
            instance.GetComponent<Portal>().PortalType = "east";
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.PORTAL);
            portalArray[0] = instance;
        }
        if (mPortalW)
        {
            instance = Instantiate(portal[(int)Stage.PORTAL_TYPE.NORMAL], tileGameObjectArray[width, height / 2].transform.position, Quaternion.Euler(0, -90, 0));
            instance.GetComponent<Portal>().PortalType = "west";
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.PORTAL);
            portalArray[1] = instance;
        }
        if (mPortalN)
        {
            instance = Instantiate(portal[(int)Stage.PORTAL_TYPE.NORMAL], tileGameObjectArray[width / 2, 1].transform.position, Quaternion.identity);
            instance.GetComponent<Portal>().PortalType = "north";
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.PORTAL);
            portalArray[2] = instance;
        }
        if (mPortalS)
        {
            instance = Instantiate(portal[(int)Stage.PORTAL_TYPE.NORMAL], tileGameObjectArray[width / 2, height].transform.position, Quaternion.Euler(0, 180, 0));
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
            instance = Instantiate(obstacle, tileGameObjectArray[i, j].transform.position, Quaternion.identity);
            instance.transform.parent = transform.GetChild((int)Stage.STAGE_CHILD.OBSTACLE);
            temp--;
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
        stage = GetComponent<Stage>();
        obstacleAmount = 0;
        portalArray = new GameObject[4];
    }
}
