using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseStateAction
{
    private EnemyMovement enemyMovement;

    public MoveAction(Entity owner, bool runUpdate) : base(owner, runUpdate)
    {
        enemyMovement = owner.Movement as EnemyMovement;
    }

    public override void Execute()
    {
        enemyMovement.MoveHorizontal();
    }
}
