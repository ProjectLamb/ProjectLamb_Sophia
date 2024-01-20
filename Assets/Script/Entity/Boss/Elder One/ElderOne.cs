using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElderOne : Boss
{
    [HideInInspector]
    public string[] triggerArray;
    [HideInInspector]
    public string[] boolArray;

    public bool IsLook = false;
    public int attackRange = 30;

    bool IsTrigger = false;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        triggerArray = new string[9] { "DoAttackLeft", "DoAttackRight", "DoAttackBoth", "DoAttackUpperCut", "DoAttackBoth2",
        "DoAttackCharge", "DoAttackWalkCharge", "DoAttackWalk", "DoAttackWalkEnd"};
        boolArray = new string[4] { "IsWalk", "IsAttack", "IsCharge", "IsAttackWalk" };

    }
    void Start()
    {
        behaviorTree.GetComponent<BehaviorTree>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fov.IsRecog)
            behaviorTree.GetComponent<BT_ElderOne>().blackBoard.boolDict["HasTarget"] = true;
        else
            behaviorTree.GetComponent<BT_ElderOne>().blackBoard.boolDict["HasTarget"] = false;

        //Debug.Log(behaviorTree.GetComponent<BT_ElderOne>().blackBoard.intDict["AttackCount"]);

        // if (CurrentHealth <= (FinalData.MaxHP / 2) && !IsTrigger)
        // {
        //     behaviorTree.GetComponent<BehaviorTree>().blackBoard.boolDict["Phase"] = true;
        //     IsTrigger = true;
        // }
    }
}