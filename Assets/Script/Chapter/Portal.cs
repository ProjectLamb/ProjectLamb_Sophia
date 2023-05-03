using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player")
        {
            WarpPortal();
            Debug.Log("S");
        }
    }
    bool portal;
    bool visited;
    Vector3 warpPos;
    GameObject map;
    GameObject departStage;
    GameObject arriveStage;
    string mPortalType;
    string PortalType
    {
        get{
            return mPortalType;
        }
        set{
            portal = true;
            mPortalType = value;
        }
    }
    public void WarpPortal()
    {
        
    }

    void Awake()
    {
        portal = false;
        visited = false;
        mPortalType = "";
    }
}
