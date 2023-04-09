using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public void SetPortalType(string s)
    {
        portal = true;
        portalType = s;
    }
    public string GetPortalType()
    {
        return portalType;
    }
    bool portal;
    bool visited;
    Vector3 warpPos;
    GameObject map;
    GameObject departStage;
    GameObject arriveStage;
    [SerializeField]
    string portalType;
    public void WarpPortal()
    {
        Debug.Log("워프 포탈 만들기");
        if (!visited)
        {
            map = transform.parent.parent.gameObject;
            departStage = transform.parent.gameObject;
            arriveStage = departStage;
            int interval = departStage.GetComponent<StageGenerator>().GetMaxSize();
            Vector3 arriveStagePos = arriveStage.transform.position;
            Vector3 destPos = new Vector3(arriveStagePos.x, 1, arriveStagePos.z);
            string arrivePortalType = "";

            if (portalType == "east")
            {
                arrivePortalType = "west";
                arriveStagePos = new Vector3(departStage.transform.position.x - interval, 0, departStage.transform.position.z);
            }
            else if (portalType == "west")
            {
                arrivePortalType = "east";
                arriveStagePos = new Vector3(departStage.transform.position.x + interval, 0, departStage.transform.position.z);
            }
            else if (portalType == "north")
            {
                arrivePortalType = "south";
                arriveStagePos = new Vector3(departStage.transform.position.x, 0, departStage.transform.position.z - interval);
            }
            else if (portalType == "south")
            {
                arrivePortalType = "north";
                arriveStagePos = new Vector3(departStage.transform.position.x, 0, departStage.transform.position.z + interval);
            }
            else
            {
                arriveStagePos = departStage.transform.position;
            }

            for (int i = 0; i < map.GetComponent<ChapterGenerator>().stageArray.Count; i++)
            {
                if (map.GetComponent<ChapterGenerator>().stageArray[i].GetComponent<StageGenerator>().GetStageLocation() != arriveStagePos)
                    continue;
                arriveStage = map.GetComponent<ChapterGenerator>().stageArray[i];
            }

            for (int i = 1; i <= arriveStage.GetComponent<StageGenerator>().GetWidth(); i++)
            {
                for (int j = 1; j <= arriveStage.GetComponent<StageGenerator>().GetWidth(); j++)
                {
                    if (arriveStage.GetComponent<StageGenerator>().tileArray[i, j].GetComponent<Tile>().GetPortalType() != arrivePortalType)
                        continue;

                    string type = arriveStage.GetComponent<StageGenerator>().tileArray[i, j].GetComponent<Tile>().GetPortalType();
                    if (type == "east")
                    {
                        destPos = arriveStage.GetComponent<StageGenerator>().tileArray[i + 1, j].transform.position;
                    }
                    else if (type == "west")
                    {
                        destPos = arriveStage.GetComponent<StageGenerator>().tileArray[i - 1, j].transform.position;
                    }
                    else if (type == "north")
                    {
                        destPos = arriveStage.GetComponent<StageGenerator>().tileArray[i, j + 1].transform.position;
                    }
                    else if (type == "south")
                    {
                        destPos = arriveStage.GetComponent<StageGenerator>().tileArray[i, j - 1].transform.position;
                    }
                }
            }
            warpPos = new Vector3(destPos.x, GameManager.Instance.playerGameObject.transform.position.y, destPos.z);

            Debug.Log(warpPos);
        }
        departStage.GetComponent<StageGenerator>().SetOffStage();
        arriveStage.GetComponent<StageGenerator>().SetOnStage();
        GameManager.Instance.playerGameObject.transform.position = warpPos;
        visited = true;
    }

    void Awake()
    {
        portal = false;
        visited = false;
        portalType = "";
    }
}