using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour
{
    bool random()
    {
        int n = Random.Range(0, 2);
        if (n == 1)
            return true;
        else
            return false;
    }
    public void SetRoomType(string s)
    {
        this.type = s;
    }
    public void SetPortal(bool e, bool w, bool s, bool n)
    {
        if (e)
            portalE = true;
        if (w)
            portalW = true;
        if (s)
            portalS = true;
        if (n)
            portalN = true;
    }

    public void SetRoomLocation(float x, float z)
    {
        this.x = x;
        this.z = z;
    }
    public int GetMaxSize()
    {
        maxSize = (initWidth + 3 * initIncrease) * (initWidth + 3 * initIncrease);
        return maxSize;
    }

    public Vector3 GetRoomLocation()
    {
        Vector3 roomPos = new Vector3(this.x, 0, this.z);
        return roomPos;
    }

    public int GetWidth()
    {
        return width;
    }
    int initWidth = 10;
    int initIncrease = 5;
    int width;
    int increase;
    int height;
    int wallHeight = 5;
    int size;
    int maxSize;
    float x;
    float z;
    int roomSizeRandom;
    [SerializeField]
    string type;
    int mobCount;
    int currentMobCount;
    bool mIsClear;
    bool isClear {
        get { return mIsClear; }
        set { mIsClear = value; }
    }
    private bool portalE = false;
    private bool portalW = false;
    private bool portalS = false;
    private bool portalN = false;
    public GameObject tile;
    public GameObject wall;
    public GameObject transWall;
    public GameObject[,] tileArray;
    public GameObject mob;
    public List<GameObject> mobArray;
    GameObject RoomCamera;

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
                instance.transform.parent = transform;
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
                            wallInstance.transform.parent = transform;
                        }
                    }
                    if (i == width)
                    {
                        for (int k = 0; k < wallHeight; k++)
                        {
                            wallPos = new Vector3(pos.x + interval, (interval / 2) + interval * k, pos.z);
                            wallInstance = Instantiate(wall, wallPos, Quaternion.identity);
                            wallInstance.transform.parent = transform;
                        }
                    }
                    if (j == width)
                    {
                        wallPos = new Vector3(pos.x, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform;
                    }
                    if (i == 1)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform;
                    }

                    if (i == 1 && j == 1)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z - interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform;
                    }

                    if (i == 1 && j == width)
                    {
                        wallPos = new Vector3(pos.x - interval, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform;
                    }

                    if (i == width && j == 1)
                    {
                        wallPos = new Vector3(pos.x + interval, interval / 2, pos.z - interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform;
                    }

                    if (i == width && j == width)
                    {
                        wallPos = new Vector3(pos.x + interval, interval / 2, pos.z + interval);
                        wallInstance = Instantiate(transWall, wallPos, Quaternion.identity);
                        wallInstance.transform.parent = transform;
                    }
                }

                if (type == "shop")
                    instance.GetComponent<Renderer>().material.color = Color.blue;
                else if (type == "start")
                    instance.GetComponent<Renderer>().material.color = Color.green;
                else if (type == "boss")
                    instance.GetComponent<Renderer>().material.color = Color.red;
                else
                    instance.GetComponent<Renderer>().material.color = Color.grey;
                tileArray[i, j] = instance;
            }
        }
    }

    void InstantiatePortal()
    {
        if (portalE)
        {
            tileArray[1, height / 2 + 1].GetComponent<Renderer>().material.color = Color.cyan;
            tileArray[1, height / 2 + 1].tag = "Portal";
            tileArray[1, height / 2 + 1].GetComponent<Tile>().SetPortalType("east");
        }
        if (portalW)
        {
            tileArray[width, height / 2 + 1].GetComponent<Renderer>().material.color = Color.cyan;
            tileArray[width, height / 2 + 1].tag = "Portal";
            tileArray[width, height / 2 + 1].GetComponent<Tile>().SetPortalType("west");
        }
        if (portalN)
        {
            tileArray[width / 2 + 1, 1].GetComponent<Renderer>().material.color = Color.cyan;
            tileArray[width / 2 + 1, 1].tag = "Portal";
            tileArray[width / 2 + 1, 1].GetComponent<Tile>().SetPortalType("north");
        }
        if (portalS)
        {
            tileArray[width / 2 + 1, height].GetComponent<Renderer>().material.color = Color.cyan;
            tileArray[width / 2 + 1, height].tag = "Portal";
            tileArray[width / 2 + 1, height].GetComponent<Tile>().SetPortalType("south");
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
                instance.transform.parent = transform;
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
            instance.transform.parent = transform;
            amount--;
        }
    }

    public void SetOnRoom()
    {
        gameObject.SetActive(true);
        if (!isClear)
            GameManager.Instance.Player.GetComponent<PlayerAction>().isPortal = false;
            GameManager.Instance.CurrentRoom = this.gameObject;
        foreach (var m in mobArray)
        {
            if (m != null)
            {
                m.GetComponent<Enemy>().chase = true;
            }
        }
    }

    public void SetOffRoom()
    {
        foreach (var m in mobArray)
        {
            if (m != null)
            {
                m.GetComponent<Enemy>().chase = false;
            }
        }
        gameObject.SetActive(false);
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

    void FirstClear()
    {
        GameManager.Instance.Player.GetComponent<PlayerAction>().isPortal = true;    
        isClear = true;
    }

    public bool GetIsClear()
    {
        return isClear;
    }
    void Awake()
    {
        width = initWidth;
        increase = initIncrease;
        //RoomCamera = transform.GetChild(0).gameObject;
        if (type == "normal")
            roomSizeRandom = Random.Range(1, 4);
        else
            roomSizeRandom = 1;
        width += increase * roomSizeRandom;
        height = width;
        tileArray = new GameObject[width + 1, height + 1];
        mobCount = Random.Range(3, 3 + 3);
        isClear = false;
    }
    void Start()
    {
        //RoomCamera.transform.position = new Vector3(RoomCamera.transform.position.x - 15 * (roomSizeRandom - 1), RoomCamera.transform.position.y, RoomCamera.transform.position.z + 15 * (roomSizeRandom - 1));
        InstantiateTile(width, height);
        InstantiatePortal();
        size = width * height;
        GenerateNevMesh();
        if (type == "normal")
        {
            InstantiateObstacle();
            InstantiateMob(mobCount);
        }
        currentMobCount = mobArray.Count;
        if (type == "start")
        {
            GameObject character = GameManager.Instance.Player;
            GameManager.Instance.CurrentRoom = this.gameObject;
            isClear = true;
            //character.transform.position = new Vector3(transform.localPosition.x, GameObject.Find("Character").transform.position.y, transform.localPosition.z);
            character.transform.position = new Vector3(transform.position.x, character.transform.position.y, transform.position.z);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    void Update()
    {
        if (!isClear)
        {
            if (currentMobCount == 0 && GameManager.Instance.CurrentRoom == this.gameObject)
            {
                FirstClear();
            }
        }
    }
}
