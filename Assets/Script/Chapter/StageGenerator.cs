using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageGenerator : MonoBehaviour
{
    int initWidth = 10;
    int initIncrease = 5;
    int width;
    int increase;
    int height;
    int wallHeight = 5;
    int size;
    int maxSize;
    int stageSizeRandom;
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
    int currentMobCount;
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
    public GameObject portal;
    public GameObject tile;
    public GameObject wall;
    public GameObject transWall;
    public GameObject[,] tileArray;
    public GameObject mob;
    public List<GameObject> mobArray;
    public GameObject[] portalArray;

    public void SetPortal(bool e, bool w, bool s, bool n)
    {
        if (e)
            mPortalE = true;
        if (w)
            mPortalW = true;
        if (s)
            mPortalS = true;
        if (n)
            mPortalN = true;
    }
    public int GetMaxSize()
    {
        maxSize = (initWidth + 3 * initIncrease) * (initWidth + 3 * initIncrease);
        return maxSize;
    }
    void InstantiateTile(int r, int c)
    {
        float interval = 10f;   //tile interval
        float x = transform.position.x - interval * (r / 2);
        float z = transform.position.z - interval * (c / 2);
        for (int i = 1; i <= r; i++)
        {
            for (int j = 1; j <= c; j++)
            {
                GameObject instance;
                Vector3 pos = new Vector3(x + interval * i, 0, z + interval * j);
                instance = Instantiate(tile, pos, Quaternion.identity);
                instance.transform.parent = transform.GetChild(0);
                /*if (Random.Range(1, 11) == 1)
                    InstantiateObstacle(pos.x,pos.z);*/
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

                if (mType == "shop")
                    instance.GetComponent<Renderer>().material.color = Color.blue;
                else if (mType == "start")
                    instance.GetComponent<Renderer>().material.color = Color.green;
                else if (mType == "boss")
                    instance.GetComponent<Renderer>().material.color = Color.red;
                else if (mType == "hidden")
                    instance.GetComponent<Renderer>().material.color = Color.black;
                else if (mType == "middleboss")
                    instance.GetComponent<Renderer>().material.color = Color.yellow;
                else
                    instance.GetComponent<Renderer>().material.color = Color.grey;
                tileArray[i, j] = instance;
            }
        }
    }

    void InstantiatePortal()
    {
        GameObject instance;
        if (mPortalE)
        {
            instance = Instantiate(portal, tileArray[1, height / 2 + 1].transform.position, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Portal>().PortalType = "east";
            //instance.transform.GetComponent<Portal>().PortalType = "east";
            instance.transform.parent = transform.GetChild(2);
            portalCount++;
            portalArray[0] = instance;
            // tileArray[1, height / 2 + 1].GetComponent<Renderer>().material.color = Color.cyan;
            // tileArray[1, height / 2 + 1].tag = "Portal";
            // tileArray[1, height / 2 + 1].GetComponent<Tile>().SetPortalType("east");
        }
        if (mPortalW)
        {
            instance = Instantiate(portal, tileArray[width, height / 2 + 1].transform.position, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Portal>().PortalType = "west";
            //instance.transform.GetComponent<Portal>().PortalType = "west";
            instance.transform.parent = transform.GetChild(2);
            portalCount++;
            portalArray[1] = instance;
            // tileArray[width, height / 2 + 1].GetComponent<Renderer>().material.color = Color.cyan;
            // tileArray[width, height / 2 + 1].tag = "Portal";
            // tileArray[width, height / 2 + 1].GetComponent<Tile>().SetPortalType("west");
        }
        if (mPortalN)
        {
            instance = Instantiate(portal, tileArray[width / 2 + 1, 1].transform.position, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Portal>().PortalType = "north";
            //instance.transform.GetComponent<Portal>().PortalType = "north";
            instance.transform.parent = transform.GetChild(2);
            portalCount++;
            portalArray[2] = instance;
            // tileArray[width / 2 + 1, 1].GetComponent<Renderer>().material.color = Color.cyan;
            // tileArray[width / 2 + 1, 1].tag = "Portal";
            // tileArray[width / 2 + 1, 1].GetComponent<Tile>().SetPortalType("north");
        }
        if (mPortalS)
        {
            instance = Instantiate(portal, tileArray[width / 2 + 1, height].transform.position, Quaternion.identity);
            instance.transform.GetChild(0).GetComponent<Portal>().PortalType = "south";
            //instance.transform.GetComponent<Portal>().PortalType = "south";
            instance.transform.parent = transform.GetChild(2);
            portalCount++;
            portalArray[3] = instance;
            // tileArray[width / 2 + 1, height].GetComponent<Renderer>().material.color = Color.cyan;
            // tileArray[width / 2 + 1, height].tag = "Portal";
            // tileArray[width / 2 + 1, height].GetComponent<Tile>().SetPortalType("south");
        }
    }

    void InstantiateObstacle()
    {
        for (int i = 1; i <= width; i++)
        {
            for (int j = 1; j <= height; j++)
            {
                if (tileArray[i, j].tag == "Portal")
                    continue;
                if (Random.Range(1, 11) > 1) //10%
                    continue;
                GameObject instance;
                instance = Instantiate(wall, tileArray[i, j].transform.position, Quaternion.identity);
                instance.transform.parent = transform.GetChild(3);
            }
        }
    }

    void InstantiateMob(int amount)
    {
        while (amount > 0)
        {
            int i = Random.Range(1, width + 1);
            int j = Random.Range(1, height + 1);
            if (tileArray[i, j].tag == "Portal")
                continue;
            GameObject instance;
            instance = Instantiate(mob, tileArray[i, j].transform.position, Quaternion.identity);
            mobArray.Add(instance);
            instance.transform.parent = transform.GetChild(4);
            amount--;
        }
    }

    public void SetOnStage()
    {
        if (!mIsClear)
            GameManager.Instance.playerGameObject.GetComponent<Player>().IsPortal = false;
        GameManager.Instance.currentStage = this.gameObject;
        foreach (var m in mobArray)
        {
            if (m != null)
            {
                m.GetComponent<Enemy>().chase = true;
            }
        }
        for(int i = 0; i < transform.childCount; i++)
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
                m.GetComponent<Enemy>().chase = false;
            }
        }
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void DecreaseCurrentMobCount()
    {
        currentMobCount--;
    }

    void GenerateNevMesh()
    {
        NavMeshSurface nav = GetComponent<NavMeshSurface>();
        nav.RemoveData();
        nav.BuildNavMesh();
    }

    void Awake()
    {
        width = initWidth;
        increase = initIncrease;
        if (mType == "normal")
            stageSizeRandom = Random.Range(1, 4);
        else
            stageSizeRandom = 1;
        width += increase * stageSizeRandom;
        height = width;
        tileArray = new GameObject[width + 1, height + 1];
        mobCount = Random.Range(3, 3 + 3);
        mIsClear = false;
        portalArray = new GameObject[4];
    }
    void Start()
    {
        InstantiateTile(width, height);
        InstantiatePortal();
        size = width * height;
        GenerateNevMesh();
        if (mType == "normal")
        {
            InstantiateObstacle();
            InstantiateMob(mobCount);
        }
        if (mType == "middleboss")
        {
            //InstantiateObstacle();
            InstantiateMob(mobCount);
        }
        else if (mType == "hidden")
        {
            //InstantiateObstacle();
            InstantiateMob(mobCount);
        }
        else if (mType == "boss")
        {

        }
        currentMobCount = mobArray.Count;
        if (mType == "start")
        {
            GameObject character = GameManager.Instance.playerGameObject;
            GameManager.Instance.currentStage = this.gameObject;
            mIsClear = true;
            //character.transform.position = new Vector3(transform.localPosition.x, GameObject.Find("Character").transform.position.y, transform.localPosition.z);
            character.transform.position = new Vector3(transform.position.x, character.transform.position.y, transform.position.z);
        }
        else
        {
            SetOffStage();
        }

    }

    void Update()
    {
        if (!mIsClear)
        {
            if (currentMobCount == 0 && GameManager.Instance.currentStage == this.gameObject)
            {
                mIsClear = true;
            }
        }
    }
}
