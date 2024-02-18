using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

public class Boss : Enemy
{

#region SerailizeMemver

    [SerializeField] protected Sophia.Composite.RecognizeEntityComposite recognize;
    [SerializeField] protected Sophia.Composite.SerialFieldOfViewData serialFieldOfViewData;

#endregion

#region Member

    protected int phase = 1;

#endregion

    protected override void Awake()
    {
        base.Awake();
        recognize = new Sophia.Composite.RecognizeEntityComposite(this.gameObject, this.serialFieldOfViewData);
    }
    void Start()
    {
        
    }
}
