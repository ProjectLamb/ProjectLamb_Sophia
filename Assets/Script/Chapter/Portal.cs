using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODPlus;

public class Portal : MonoBehaviour
{
    public CommandSender portalSFXSource;
    public Animator animator;
    public bool IsCollider;
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
    private float mWarpInterval;
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
                    mWarpPos = new Vector3(transform.position.x + mWarpInterval, 0, transform.position.z);
                    break;
                case "west":
                    mWarpPos = new Vector3(transform.position.x - mWarpInterval, 0, transform.position.z);
                    break;
                case "south":
                    mWarpPos = new Vector3(transform.position.x, 0, transform.position.z - mWarpInterval);
                    break;
                case "north":
                    mWarpPos = new Vector3(transform.position.x, 0, transform.position.z + mWarpInterval);
                    break;
            }
        }
    }
    public void WarpPortal()
    {
        departStage = GameManager.Instance.CurrentStage;
        if (!visited)
        {
            string arrivePortalType = "";
            int CurrentStageNumber = GameManager.Instance.CurrentStage.GetComponent<Stage>().StageNumber;
            switch (mPortalType)
            {
                case "east":
                    arrivePortalType = "west";
                    arriveStage = map.GetComponent<ChapterGenerator>().stage[CurrentStageNumber].East.stageObject;
                    break;
                case "west":
                    arrivePortalType = "east";
                    arriveStage = map.GetComponent<ChapterGenerator>().stage[CurrentStageNumber].West.stageObject;
                    break;
                case "south":
                    arrivePortalType = "north";
                    arriveStage = map.GetComponent<ChapterGenerator>().stage[CurrentStageNumber].South.stageObject;
                    break;
                case "north":
                    arrivePortalType = "south";
                    arriveStage = map.GetComponent<ChapterGenerator>().stage[CurrentStageNumber].North.stageObject;
                    break;
            }
            newWarpPos = transform.position;
            switch (arrivePortalType)
            {
                case "east":
                    newWarpPos = arriveStage.GetComponent<StageGenerator>().portalArray[0].GetComponent<Portal>().WarpPos;
                    break;
                case "west":
                    newWarpPos = arriveStage.GetComponent<StageGenerator>().portalArray[1].GetComponent<Portal>().WarpPos;
                    break;
                case "north":
                    newWarpPos = arriveStage.GetComponent<StageGenerator>().portalArray[2].GetComponent<Portal>().WarpPos;
                    break;
                case "south":
                    newWarpPos = arriveStage.GetComponent<StageGenerator>().portalArray[3].GetComponent<Portal>().WarpPos;
                    break;
            }
            visited = true;
        }
        GameManager.Instance.GlobalEvent.PlayerMoveStage(departStage, arriveStage, newWarpPos);
    }

    void Awake()
    {
        IsCollider = false;
        animator = GetComponentInChildren<Animator>();
        visited = false;
        mPortalType = "";
        mWarpInterval = transform.localScale.x;
        map = GameManager.Instance.ChapterGenerator;
        mWarpPos = transform.position;
    }
    void Start()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == GameManager.Instance.PlayerGameObject)
        {
            if (IsCollider){
                WarpPortal();
                portalSFXSource.SendCommand();
            }
        }
    }
}