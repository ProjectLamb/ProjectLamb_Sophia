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
            new SequenceNode(new List<Node> //����� �����Ǿ�����
            {
                new BTT_CheckBool(transform, "HasTarget"),
                new SelectorNode(new List<Node>
                {
                    new SequenceNode(new List<Node>
                    {
                        new BTT_DistanceToTarget(transform, elderOne.objectiveTarget, "Distance"),  //���� �����Ÿ� ��
                        new SelectorNode(new List<Node>
                        {
                            /*new BTT_EO_Attack(transform),
                            new BTT_EO_Attack2(transform)*/
                        })
                    }),

                    new SequenceNode(new List<Node> //�߰� ������
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
