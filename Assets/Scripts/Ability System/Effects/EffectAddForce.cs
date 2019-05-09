using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAddForce : Effect
{

    private AddForceInfo forceInfo;

    private Vector2 resultingDirection;

    public EffectAddForce(Ability parentAbility, AddForceInfo forceInfo) : base(parentAbility)
    {
        this.forceInfo = forceInfo;
    }



    public override void Apply(GameObject target)
    {
        base.Apply(target);

        switch (durationType)
        {
            case Constants.EffectDurationType.Instant:
                ApplyInstantForce(target);
                break;

            case Constants.EffectDurationType.Duration:
            case Constants.EffectDurationType.Periodic:
                CreateAndRegisterStatus(target);
                break;

        }
    }


    protected override bool CreateAndRegisterStatus(GameObject target)
    {
        StatusMovementAffecting newStatus = new StatusMovementAffecting(statusInfo, forceInfo);
        //Debug.Log(newStatus.statusType + " has been made from " + effectName + " on " + parentAbility.abilityName);

        activeStatus.Add(newStatus);
        StatusManager.AddStatus(target, newStatus);

        return true;
    }


    private void ApplyInstantForce(GameObject target)
    {
        Vector2 forceToAdd = forceInfo.CalcDirectionAndForce(target, Source);

        //Debug.Log("adding " + forceToAdd);

        if (forceInfo.resetCurrentVelocity == true)
            target.Entity().Movement.MyBody.velocity = Vector2.zero;

        target.Entity().Movement.MyBody.AddForce(forceToAdd);
    }







}


[System.Serializable]
public struct AddForceInfo {

    public enum DirectionType {
        AwayInLine,
        Up,
        Down,
        Forward,
        Backward,
        CustomAngle

    }


    public DirectionType directionType;
    //public Vector2 direction;
    public float amount;
    public float angle;
    public float error;
    public bool resetCurrentVelocity;

    public AddForceInfo(DirectionType directionType, float amount, float angle = 0f,float error = 0f, bool resetCurrentVelocity = false)
    {
        this.amount = amount;
        this.directionType = directionType;
        this.angle = angle;
        this.resetCurrentVelocity = resetCurrentVelocity;
        this.error = error;
    }


    public Vector2 CalcDirectionAndForce(GameObject target, GameObject source)
    {
        Vector2 result = Vector2.zero;

        switch (directionType)
        {
            case DirectionType.AwayInLine:
                result = (target.transform.position - source.transform.position).normalized;

                Debug.Log((result * amount) + " is being applied");
                break;

            case DirectionType.Up:
                result = Vector2.up;
                break;

            case DirectionType.Down:
                result = Vector2.down;
                break;

            case DirectionType.Forward:
                result = source.Entity().Movement.Facing == EntityMovement.FacingDirection.Left ? Vector2.left : Vector2.right;
                break;

            case DirectionType.Backward:
                result = source.Entity().Movement.Facing == EntityMovement.FacingDirection.Left ? Vector2.right : Vector2.left;
                break;

            case DirectionType.CustomAngle:

                float offset = Random.Range(-error, error);

                //Debug.Log(angle + " ANGLE");
                //Debug.Log((angle + offset) + " OFFSET ANGLE");

                Vector2 conversion = TargetingTools.DegreeToVector2(angle + offset);

                if(source != null)
                    conversion = source.Entity().Movement.Facing == EntityMovement.FacingDirection.Left ? new Vector2(-conversion.x, conversion.y) : conversion;

                result = conversion;

                break;
        }

        result *= amount;

        return result;
    }

}
