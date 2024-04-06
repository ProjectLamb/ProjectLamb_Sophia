using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSet : MonoBehaviour
{
    GameObject westWall;
    GameObject northWall;
    // Start is called before the first frame update
    void Awake()
    {
        westWall = transform.GetChild(0).gameObject;
        northWall = transform.GetChild(1).gameObject;
    }

    public void SetWestPortal()
    {
        westWall.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetNorthPortal()
    {
        northWall.transform.GetChild(0).gameObject.SetActive(false);
    }
}
