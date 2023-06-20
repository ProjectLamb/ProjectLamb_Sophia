using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public Transform chrTr;
    public Vector3 offset;

    // Update is called once per frame
    void Awake()
    {
        chrTr = GameManager.Instance.playerGameObject.transform;
    }
    void Update()
    {
        this.transform.position = chrTr.position+offset;
    }
}
