using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectOrigin = Constants.EffectOrigin;
using EffectDeliveryMethod = Constants.EffectDeliveryMethod;
using EffectTag = Constants.EffectTag;
using EffectType = Constants.EffectType;

public static class EffectFactory {


    public static Effect CreateEffect(Ability parent, EffectData data)
    {
        Effect result = new Effect(parent); 



        switch (data.effectType)
        {
            case EffectType.StatAdjustment:
                result = new EffectStatAdjustment(parent, data.adjInfo);
                break;

            case EffectType.AddForce:
                result = new EffectAddForce(parent, data.addForceInfo);
                break;
        }


        result.effectName = data.effectName;
        result.riderTarget = data.riderTarget;
        result.tags = data.tags;
        result.effectOrigin = data.effectOrigin;
        result.statusTypeInfo = data.statusTypeInfo;
        result.deliveryMethod = data.deliveryMethod;
        result.layerMask = data.layerMask;
        result.effectZoneInfo = data.effectZoneInfo;
        result.animationTrigger = data.animationTrigger;
        result.durationType = data.durationType;
        result.projectileInfo = data.projectileInfo;
        result.weaponDelivery = data.weaponDelivery;
        result.weaponPrefabName = data.weaponPrefabName;
        result.weaponDeliveryAnimSpeed = data.weaponDeliveryAnimSpeed;
        result.weaponAnimTrigger = data.weaponAnimTrigger;

        return result;
    }


    public static AbilityRecovery CreateRecovery(Ability parent, RecoveryData data)
    {
        AbilityRecovery result = null;

        switch (data.type)
        {
            case Constants.AbilityRecoveryType.Cooldown:
                result = new RecoveryCooldown(data.cooldown, parent, parent.RecoveryManager);
                break;

            default:

                break;

        }


        return result;

    }





}
