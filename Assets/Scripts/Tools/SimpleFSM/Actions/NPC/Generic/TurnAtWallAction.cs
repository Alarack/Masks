using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAtWallAction : BaseStateAction
{

    EnemyMovement enemyMovement;

    public TurnAtWallAction(Entity owner, bool runUpdate):base(owner, runUpdate)
    {
        enemyMovement = owner.Movement as EnemyMovement;
    }

    public override void Execute()
    {
        if (enemyMovement.RayController.IsHittingWall)
        {
            enemyMovement.FlipDirection();
            //Debug.Log("Wall flip");
        }

    }

}
