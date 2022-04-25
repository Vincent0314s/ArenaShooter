using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils;

public class Unit_Enemy : Unit_Base
{
    public EnemyPath path;
    protected override void Start()
    {
        base.Start();
        MoveToDestination(path.endPoints[0]);
    }

    public void InitStartPoint()
    {
        transform.position = path.startPoint;
    }

    public override void MoveToDestination(Vector3 _clickPoint)
    {
        agent.SetDestination(_clickPoint);
    }

    protected override void Update()
    {
        base.Update();
        RepeatePath();
    }

    private void RepeatePath() {
        if (Vector3.Distance(transform.position,agent.destination) < 0.25f) {
            InitStartPoint();
        }
    }
}
