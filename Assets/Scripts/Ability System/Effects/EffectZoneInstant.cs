using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectZoneInstant : EffectZone {



    public override void Initialize(Effect parentEffect, LayerMask mask, Transform parentToThis = null)
    {
        base.Initialize(parentEffect, mask, parentToThis);

        Invoke("CleanUp", parentEffect.effectZoneInfo.instantZoneLife);
    }


    protected override void OnTriggerStay(Collider other)
    {
        ApplyAfterLayerCheck(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ApplyAfterLayerCheck(other.gameObject);
    }


    protected override void Apply(GameObject target)
    {
        //Debug.Log("attempting to apply an effect from " + parentEffect.effectName);

        if (CheckHitTargets(target) == false)
            return;

        //Debug.Log("Applying an effect from " + parentEffect.effectName);

        Vector3 impactPoint = Vector3.zero;

        if (target.Entity() == null)
            impactPoint = transform.position;
        else
            impactPoint = target.transform.position;
        
        CreateImpactEffect(impactPoint);

        if (parentEffect != null)
            parentEffect.Apply(target);
        else
            Debug.LogError("This effect zone: " + gameObject.name + " has no parent effect");
    }


    protected override void Remove(GameObject target)
    {
        targets.RemoveIfContains(target);
    }







}
