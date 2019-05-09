using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusMovementAffecting : Status {

    private float baseForceAmount;
    private AddForceInfo forceInfo;

    public StatusMovementAffecting(StatusInfo info, AddForceInfo forceInfo) : base(info)
    {
        SetupMovement(forceInfo);
    }


    private void SetupMovement(AddForceInfo forceInfo)
    {
        this.baseForceAmount = forceInfo.amount;
        this.forceInfo = forceInfo;
    }


    private void ApplyForce()
    {
        Vector2 knockback = forceInfo.CalcDirectionAndForce(Target, Source);

        if(Target != null)
        {
            if (forceInfo.resetCurrentVelocity == true)
                Target.Entity().Movement.MyBody.velocity = Vector2.zero;

            //Debug.Log(knockback + " has been applied from " + SourceEffect.effectName + " on " + SourceAbility.abilityName);

            Target.Entity().Movement.MyBody.AddForce(knockback);
        }
    }


    protected override void Tick()
    {
        ApplyForce();
    }

    public override void Stack()
    {
        base.Stack();

        //forceInfo.amount += baseForceAmount;
        ApplyForce();

    }

    protected override void CleanUp()
    {
        base.CleanUp();
    }


}
