using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetAction : BaseStateAction
{

    private AISensor aiSensor;

    public ChaseTargetAction(Entity owner, bool runUpdate):base(owner, runUpdate)
    {
        aiSensor = ((EntityEnemy)owner).AISensor;
    }


    public override void Execute()
    {
        FaceTarget();
    }


    private void FaceTarget()
    {
        //Debug.Log(aiSensor == null);

        GameObject closestTarget = aiSensor.ClosestTarget;

        if (closestTarget == null)
            return;

        owner.Movement.SetFacing(GetProperFacing(closestTarget.transform));

    }

    private EntityMovement.FacingDirection GetProperFacing(Transform targetPos)
    {
        if(targetPos.position.x < owner.transform.position.x)
        {
            return EntityMovement.FacingDirection.Left;
        }
        else
        {
            return EntityMovement.FacingDirection.Right;
        }
    }

}
