using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_ElderOne : BehaviorTree
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
                            new SequenceNode(new List<Node>
                            {
                                new BTT_CheckInt(transform, "AttackCount", 3),
                                new SelectorNode(new List<Node> //스킬 셀렉터
                                {
                                    new SequenceNode(new List<Node>
                                    {
                                        new BTT_CheckBool(transform, "Phase"),
                                        //new BTT_AttackPhase2
                                    }),


                                    new SequenceNode(new List<Node> //페이즈1 스킬
                                    {
                                        new BTT_EO_MoveOff(transform),
                                        new BTT_TurnToTarget(transform, elderOne.objectiveTarget, 1),
                                        new BTT_EO_UpperCut(transform),
                                    })
                                })
                            }),

                            new SelectorNode(new List<Node> //평타 셀렉터
                            {
                                new SequenceNode(new List<Node> //페이즈2 평타
                                {
                                    new BTT_CheckBool(transform, "Phase"),
/*                                    new BTT_EO_MoveOff(transform),
                                    new BTT_TurnToTarget(transform, elderOne.objectiveTarget, 2),
                                    new BTT_EO_Attack2(),*/
                                }),

                                new SequenceNode(new List<Node> //페이즈1 평타
                                {
                                    new BTT_EO_MoveOff(transform),
                                    new BTT_TurnToTarget(transform, elderOne.objectiveTarget, 1),
                                    new BTT_EO_Attack(transform),
                                })
                            })
                        })
                    }),

                    new SequenceNode(new List<Node> //추격 시퀀스
                    {
                        new BTT_TurnToTarget(transform, elderOne.objectiveTarget, 2),
                        new BTT_MoveTo(transform, elderOne.objectiveTarget),
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
        bb.boolDict.Add("Phase", false);
        bb.floatDict.Add("Distance", elderOne.range);
        bb.intDict.Add("AttackCount", 0);

        return bb;
    }
}
