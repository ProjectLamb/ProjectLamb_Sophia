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
    GameObject departRoom;
    GameObject arriveRoom;
    [SerializeField]
    string portalType;
    public void WarpPortal()
    {
        if (!visited)
        {
            map = transform.parent.parent.gameObject;
            departRoom = transform.parent.gameObject;
            arriveRoom = departRoom;
            int interval = departRoom.GetComponent<RoomGenerator>().GetMaxSize();
            Vector3 arriveRoomPos = arriveRoom.transform.position;
            Vector3 destPos = new Vector3(arriveRoomPos.x, 1, arriveRoomPos.z);
            string arrivePortalType = "";

            if (portalType == "east")
            {
                arrivePortalType = "west";
                arriveRoomPos = new Vector3(departRoom.transform.position.x - interval, 0, departRoom.transform.position.z);
            }
            else if (portalType == "west")
            {
                arrivePortalType = "east";
                arriveRoomPos = new Vector3(departRoom.transform.position.x + interval, 0, departRoom.transform.position.z);
            }
            else if (portalType == "north")
            {
                arrivePortalType = "south";
                arriveRoomPos = new Vector3(departRoom.transform.position.x, 0, departRoom.transform.position.z - interval);
            }
            else if (portalType == "south")
            {
                arrivePortalType = "north";
                arriveRoomPos = new Vector3(departRoom.transform.position.x, 0, departRoom.transform.position.z + interval);
            }
            else
            {
                arriveRoomPos = departRoom.transform.position;
            }

            for (int i = 0; i < map.GetComponent<MapGenerator>().roomArray.Count; i++)
            {
                if (map.GetComponent<MapGenerator>().roomArray[i].GetComponent<RoomGenerator>().GetRoomLocation() != arriveRoomPos)
                    continue;
                arriveRoom = map.GetComponent<MapGenerator>().roomArray[i];
            }

            for (int i = 1; i <= arriveRoom.GetComponent<RoomGenerator>().GetWidth(); i++)
            {
                for (int j = 1; j <= arriveRoom.GetComponent<RoomGenerator>().GetWidth(); j++)
                {
                    if (arriveRoom.GetComponent<RoomGenerator>().tileArray[i, j].GetComponent<Tile>().GetPortalType() != arrivePortalType)
                        continue;

                    string type = arriveRoom.GetComponent<RoomGenerator>().tileArray[i, j].GetComponent<Tile>().GetPortalType();
                    if (type == "east")
                    {
                        destPos = arriveRoom.GetComponent<RoomGenerator>().tileArray[i + 1, j].transform.position;
                    }
                    else if (type == "west")
                    {
                        destPos = arriveRoom.GetComponent<RoomGenerator>().tileArray[i - 1, j].transform.position;
                    }
                    else if (type == "north")
                    {
                        destPos = arriveRoom.GetComponent<RoomGenerator>().tileArray[i, j + 1].transform.position;
                    }
                    else if (type == "south")
                    {
                        destPos = arriveRoom.GetComponent<RoomGenerator>().tileArray[i, j - 1].transform.position;
                    }
                }
            }
            warpPos = new Vector3(destPos.x, GameManager.Instance.Player.transform.position.y, destPos.z);
        }
        departRoom.GetComponent<RoomGenerator>().SetOffRoom();
        arriveRoom.GetComponent<RoomGenerator>().SetOnRoom();
        GameManager.Instance.Player.transform.position = warpPos;
        visited = true;
    }

    void Awake()
    {
        portal = false;
        visited = false;
        portalType = "";
    }
}
