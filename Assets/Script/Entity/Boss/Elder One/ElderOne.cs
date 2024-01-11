using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderOne : Boss
{
    public int range = 30;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

    }
    void Start()
    {
        behaviorTree.GetComponent<BehaviorTree>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fov.IsRecog)
            behaviorTree.GetComponent<ElderOneBT>().blackBoard.boolDict["HasTarget"] = true;
        else
            behaviorTree.GetComponent<ElderOneBT>().blackBoard.boolDict["HasTarget"] = false;
    }
}