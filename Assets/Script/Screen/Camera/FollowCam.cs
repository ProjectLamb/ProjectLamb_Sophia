using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public Transform chrTr;
    public Vector3 offset;

    void Awake() 
    {
        chrTr = GameManager.Instance.PlayerGameObject.transform;    
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = chrTr.position+offset;
    }
}
