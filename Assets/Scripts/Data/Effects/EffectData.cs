using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectOrigin = Constants.EffectOrigin;
using EffectDeliveryMethod = Constants.EffectDeliveryMethod;
using EffectTag = Constants.EffectTag;
using EffectType = Constants.EffectType;

[System.Serializable]
public class EffectData{

    public string effectName;
    public string riderTarget;
    public List<EffectTag> tags = new List<EffectTag>();
    public EffectOrigin effectOrigin;

    public bool weaponDelivery;
    public float weaponDeliveryAnimSpeed = 1f;
    public string weaponPrefabName;
    public string weaponAnimTrigger;


    public EffectDeliveryMethod deliveryMethod;
    public EffectType effectType;
    public Constants.EffectDurationType durationType;

    //Durational Effects / Status
    public StatusTypeInfo statusTypeInfo = new StatusTypeInfo();



    //Projectile Stuff
    public ProjectileInfo projectileInfo = new ProjectileInfo();


    public ZoneInfo effectZoneInfo = new ZoneInfo();
    public string animationTrigger;
    public LayerMask layerMask;

    //Effect Stat Adjustment
    public StatAdjustmentInfo adjInfo = new StatAdjustmentInfo();

    //Effect Add Force
    public AddForceInfo addForceInfo = new AddForceInfo();

}
