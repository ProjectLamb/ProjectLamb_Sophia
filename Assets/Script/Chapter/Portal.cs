using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (transform.parent.parent.parent.GetComponent<StageGenerator>().IsClear)
                WarpPortal();
        }
    }
    bool visited;
    [SerializeField]
    private Vector3 mWarpPos;
    public Vector3 WarpPos
    {
        get
        {
            return mWarpPos;
        }
        set
        {
            mWarpPos = value;
        }
    }

    Vector3 newWarpPos;
    GameObject map;
    GameObject departStage;
    GameObject arriveStage;
    private float warpInterval;
    [SerializeField]
    private string mPortalType;
    public string PortalType
    {
        get
        {
            return mPortalType;
        }
        set
        {
            mPortalType = value;
            switch (mPortalType)
            {
                case "east":
                    mWarpPos = new Vector3(transform.position.x + warpInterval, 0, transform.position.z);
                    break;
                case "west":
                    mWarpPos = new Vector3(transform.position.x - warpInterval, 0, transform.position.z);
                    break;
                case "south":
                    mWarpPos = new Vector3(transform.position.x, 0, transform.position.z - warpInterval);
                    break;
                case "north":
                    mWarpPos = new Vector3(transform.position.x, 0, transform.position.z + warpInterval);
                    break;
            }
        }
    }
    public void WarpPortal()
    {
        if (!visited)
        {
            string arrivePortalType = "";
            int currentStageNumber = GameManager.Instance.currentStage.GetComponent<StageGenerator>().StageNumber;
            switch (mPortalType)
            {
                case "east":
                    arrivePortalType = "west";
                    arriveStage = map.GetComponent<ChapterGenerator>().stage[currentStageNumber].East.stageObject;
                    break;
                case "west":
                    arrivePortalType = "east";
                    arriveStage = map.GetComponent<ChapterGenerator>().stage[currentStageNumber].West.stageObject;
                    break;
                case "south":
                    arrivePortalType = "north";
                    arriveStage = map.GetComponent<ChapterGenerator>().stage[currentStageNumber].South.stageObject;
                    break;
                case "north":
                    arrivePortalType = "south";
                    arriveStage = map.GetComponent<ChapterGenerator>().stage[currentStageNumber].North.stageObject;
                    break;
            }
            newWarpPos = transform.position;
            switch (arrivePortalType)
            {
                case "east":
                    newWarpPos = arriveStage.GetComponent<StageGenerator>().portalArray[0].transform.GetChild(0).GetComponent<Portal>().WarpPos;
                    break;
                case "west":
                    newWarpPos = arriveStage.GetComponent<StageGenerator>().portalArray[1].transform.GetChild(0).GetComponent<Portal>().WarpPos;
                    break;
                case "north":
                    newWarpPos = arriveStage.GetComponent<StageGenerator>().portalArray[2].transform.GetChild(0).GetComponent<Portal>().WarpPos;
                    break;
                case "south":
                    newWarpPos = arriveStage.GetComponent<StageGenerator>().portalArray[3].transform.GetChild(0).GetComponent<Portal>().WarpPos;
                    break;
            }
            visited = true;
        }
        GameManager.Instance.globalEvent.PlayerWarp(departStage, arriveStage, newWarpPos);
    }

    void Awake()
    {
        visited = false;
        mPortalType = "";
        warpInterval = transform.localScale.x;
        map = GameManager.Instance.ChapterGenerator;
        mWarpPos = transform.position;
    }
    void Start()
    {
        departStage = GameManager.Instance.currentStage;
    }

}
