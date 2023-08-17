using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Template : Enemy
{
    public override void Die()
    {
        base.Die();
        stage.mobGenerator.CurrentMobCount--;
        Invoke("DestroySelf", 0.5f);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(objectiveTarget);
        nav.SetDestination(objectiveTarget.position);
    }
}
