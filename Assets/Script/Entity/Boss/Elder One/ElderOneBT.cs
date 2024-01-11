using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderOneBT : BehaviorTree
{
    ElderOne elderOne;
    // Start is called before the first frame update
    private void Awake()
    {
        elderOne = transform.GetComponent<ElderOne>();
    }

    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override Node SetupBehaviorTree()
    {
        Node root = new SelectorNode(new List<Node>
        {
            new SequenceNode(new List<Node> //대상이 감지되었는지
            {
                new BTT_CheckBool(transform, "HasTarget"),
                new SelectorNode(new List<Node>
                {
                    new SequenceNode(new List<Node>
                    {
                        new BTT_DistanceToTarget(transform, elderOne.objectiveTarget, "Distance"),  //공격 사정거리 안
                        new SelectorNode(new List<Node>
                        {
                            /*new BTT_EO_Attack(transform),
                            new BTT_EO_Attack2(transform)*/
                        })
                    }),

                    new SequenceNode(new List<Node> //추격 시퀀스
                    {
                        new BTT_TurnToTarget(transform, elderOne.objectiveTarget),
                    })
                })
            }),

            new SequenceNode(new List<Node>
            {
                new BTT_EO_Idle(transform),
            })
        });

        return root;
    }

    protected override BlackBoard SetupBlackBoard()
    {
        BlackBoard bb = new BlackBoard();
        bb.boolDict.Add("HasTarget", false);
        bb.floatDict.Add("Distance", elderOne.range);

        return bb;
    }
}
