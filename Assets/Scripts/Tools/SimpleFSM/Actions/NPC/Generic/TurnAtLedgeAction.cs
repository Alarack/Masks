using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAtLedgeAction : BaseStateAction {

    EnemyMovement enemyMovement;

    public TurnAtLedgeAction(Entity owner, bool runUpdate) : base(owner, runUpdate)
    {
        enemyMovement = owner.Movement as EnemyMovement;
    }

    public override void Execute()
    {
        if (enemyMovement.RayController.IsAtLedge)
        {
            enemyMovement.FlipDirection();
            //Debug.Log("Edge flip");
        }
    }

}
