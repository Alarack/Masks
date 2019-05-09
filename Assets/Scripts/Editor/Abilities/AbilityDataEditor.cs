using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityData))]
public class AbilityDataEditor : Editor {

    private AbilityData _abilityData;


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _abilityData = (AbilityData)target;

        EditorGUILayout.LabelField("Ability Info", EditorStyles.boldLabel);

        _abilityData.abilityName = EditorGUILayout.TextField("Ability Name", _abilityData.abilityName);
        _abilityData.abilityIcon = EditorHelper.ObjectField<Sprite>("Icon", _abilityData.abilityIcon);
        _abilityData.useDuration = EditorHelper.FloatField("Use Time", _abilityData.useDuration);
        _abilityData.overrideOtherAbilities = EditorGUILayout.Toggle("Interupt Other Abilities?", _abilityData.overrideOtherAbilities);
        _abilityData.procChance = EditorHelper.PercentFloatField("Proc Chance", _abilityData.procChance);
        _abilityData.baseWeight = EditorHelper.FloatField("Base Weight", _abilityData.baseWeight);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Sequenced Abilities", EditorStyles.boldLabel);
        _abilityData.sequenceWindow = EditorHelper.FloatField("Sequence Window", _abilityData.sequenceWindow);
        _abilityData.sequencedAbilities = EditorHelper.DrawList("Abilities", _abilityData.sequencedAbilities, true, null, true, DrawAbilityData);
        //_abilityData.triggers = EditorHelper.DrawList("Activation Methods", _abilityData.triggers, true, Constants.AbilityActivationMethod.None, true, DrawAbilityTriggers);

        EditorGUILayout.LabelField("Activation Triggers", EditorStyles.boldLabel);

        _abilityData.activations = EditorHelper.DrawExtendedList(_abilityData.activations, "Activation", DrawActivator);

        EditorGUILayout.LabelField("Activation Conditions", EditorStyles.boldLabel);
        _abilityData.conditions = EditorHelper.DrawExtendedList(_abilityData.conditions, "Condition", DrawAbiltiyCondition);



        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);

        _abilityData.effectData = EditorHelper.DrawExtendedList(_abilityData.effectData, "Effect", DrawEffect);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Recovery", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        _abilityData.recoveryData = EditorHelper.DrawExtendedList("Recovery Types", _abilityData.recoveryData, "Recovery", DrawRecoveryData);


        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }



    private AbilityActivationInfo DrawActivator(AbilityActivationInfo entry)
    {
        entry.activationConditions = EditorHelper.DrawList("Activation Options", "Option", entry.activationConditions, true, Constants.AbilityActivationCondition.Normal, true, DrawListOfEnums);
        entry.activationMethod = EditorHelper.EnumPopup("Activation Method", entry.activationMethod);

        switch (entry.activationMethod)
        {
            case Constants.AbilityActivationMethod.Timed:
                entry.activationTime = EditorGUILayout.FloatField("Time", entry.activationTime);
                break;

            case Constants.AbilityActivationMethod.StatChanged:
                entry.targetstat = EditorHelper.EnumPopup("What Stat Changed?", entry.targetstat);
                entry.gainedOrLost = EditorHelper.EnumPopup("Gained or Lost?", entry.gainedOrLost);
                entry.statChangeTarget = EditorHelper.EnumPopup("Target of Stat Change", entry.statChangeTarget);
                break;

            case Constants.AbilityActivationMethod.Manual:
                entry.keyBind = EditorHelper.EnumPopup("Key Bind", entry.keyBind);
                break;
        }

        return entry;
    }

    private AbilityCondition DrawAbiltiyCondition(AbilityCondition entry)
    {
        entry.type = EditorHelper.EnumPopup("Condition Type", entry.type);
        entry.weightModifer = EditorHelper.FloatField("Weight Modifier", entry.weightModifer);
        entry.requiredCondition = EditorGUILayout.Toggle("Required Conditon", entry.requiredCondition);

        entry.inverse = EditorGUILayout.Toggle("Invert Condition?", entry.inverse);
        entry.compareAgainstTarget = EditorGUILayout.Toggle("Compare against Target?", entry.compareAgainstTarget);

        EditorGUILayout.Separator();

        if (entry.compareAgainstTarget)
        {
            EditorGUILayout.LabelField("Warning: if comparing against a target, this must be an enemy ability as players don't have explicitly designated targets");
        }

        switch (entry.type)
        {
            case AbilityCondition.AbilityConditionType.DistanceFromTarget:
                EditorGUILayout.LabelField("Target must be within this distance:");

                entry.targetDistance = EditorHelper.FloatField("Minimum Distance", entry.targetDistance);
                break;
            case AbilityCondition.AbilityConditionType.StatValue:
                EditorGUILayout.LabelField("Source or Target has a this stat at or above this value:");
                entry.statValueTarget = EditorHelper.EnumPopup("Stat Type", entry.statValueTarget);
                entry.statValueAmount = EditorHelper.FloatField("Stat Value", entry.statValueAmount);
                break;
            case AbilityCondition.AbilityConditionType.StatRatio:
                EditorGUILayout.LabelField("Source or Target has a this stat at or above this ratio:");
                entry.statRatioTarget = EditorHelper.EnumPopup("Stat Type", entry.statRatioTarget);
                entry.statRatioAmount = Mathf.Clamp01( EditorHelper.FloatField("Stat Ratio", entry.statRatioAmount));
                break;
            case AbilityCondition.AbilityConditionType.HasStatus:
                EditorGUILayout.LabelField("Source or Target has a status of the following type:");
                entry.hasStatusType = EditorHelper.EnumPopup("Status", entry.hasStatusType);
                break;
            case AbilityCondition.AbilityConditionType.Grounded:
                EditorGUILayout.LabelField("Source or Target is on the ground:");
                break;
            case AbilityCondition.AbilityConditionType.HasTarget:
                EditorGUILayout.LabelField("Source has a target (Enemy abilities only)");
                break;
            default:
                break;
        }

        return entry;
    }

    private EffectData DrawEffect(EffectData entry)
    {
        EditorGUILayout.LabelField("Basic Effect Info", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        entry.effectName = EditorGUILayout.TextField("Effect Name", entry.effectName);
        entry.animationTrigger = EditorGUILayout.TextField("Anim Trigger", entry.animationTrigger);
        entry.layerMask = EditorHelper.LayerMaskField("Layer Mask", entry.layerMask);
        entry.effectType = EditorHelper.EnumPopup("Effect Type", entry.effectType);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Delivery", EditorStyles.boldLabel);
        entry.deliveryMethod = EditorHelper.EnumPopup(entry.deliveryMethod);
        EditorGUILayout.Separator();
        entry.weaponDelivery = EditorGUILayout.Toggle("Weapon Delivery", entry.weaponDelivery);
        if(entry.weaponDelivery == true)
        {
            entry.weaponPrefabName = EditorGUILayout.TextField("Weapon Prefab", entry.weaponPrefabName);
            entry.weaponDeliveryAnimSpeed = EditorGUILayout.FloatField("Weapon Animation Speed", entry.weaponDeliveryAnimSpeed);
            entry.weaponAnimTrigger = EditorGUILayout.TextField("Weapon Trigger", entry.weaponAnimTrigger);
        }


        
        EditorGUILayout.Separator();
        switch (entry.deliveryMethod)
        {
            case Constants.EffectDeliveryMethod.Projectile:
                entry.projectileInfo = DrawProjectileInfo(entry.projectileInfo);
                break;

            case Constants.EffectDeliveryMethod.Rider:
                entry.riderTarget = EditorGUILayout.TextField("Target Effect Name", entry.riderTarget);
                break;
        }
        EditorGUILayout.Separator();

        switch (entry.deliveryMethod)
        {
            case Constants.EffectDeliveryMethod.None:
                break;
            case Constants.EffectDeliveryMethod.Instant:
            case Constants.EffectDeliveryMethod.Projectile:

                EditorGUILayout.LabelField("Zone Info", EditorStyles.boldLabel);
                entry.effectOrigin = EditorHelper.EnumPopup("Origin Location", entry.effectOrigin);
                entry.effectZoneInfo = DrawZoneInfo(entry.effectZoneInfo);
                EditorGUILayout.Separator();

                break;
    
            case Constants.EffectDeliveryMethod.SelfTargeting:
                break;
            case Constants.EffectDeliveryMethod.ExistingTargets:
                break;
            case Constants.EffectDeliveryMethod.Rider:
                break;
            default:
                break;
        }
        

        EditorGUILayout.LabelField("Duration", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        entry.durationType = EditorHelper.EnumPopup(entry.durationType);

        switch (entry.durationType)
        {
            case Constants.EffectDurationType.Duration:
            case Constants.EffectDurationType.Periodic:
                entry.statusTypeInfo = DrawStatusTypeInfo(entry.statusTypeInfo);
                break;
        }
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Specific Effect", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        switch (entry.effectType)
        {
            case Constants.EffectType.StatAdjustment:
                entry.adjInfo = DrawStatAdjustmentInfo(entry.adjInfo);
                break;

            case Constants.EffectType.AddForce:
                entry.addForceInfo = DrawAddForceInfo(entry.addForceInfo);
                break;
        }

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Tags", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        entry.tags = EditorHelper.DrawList("Tags", "Tag", entry.tags, true, Constants.EffectTag.None, true, DrawListOfEnums);

        return entry;
    }


    private ZoneInfo DrawZoneInfo(ZoneInfo entry)
    {
        entry.durationType = EditorHelper.EnumPopup("Zone Duration Type", entry.durationType);

        if(entry.durationType == EffectZone.EffectZoneDuration.Persistant)
        {
            EditorGUILayout.Separator();
            entry.duration = EditorHelper.FloatField("Duration (0 = INF)", entry.duration);
            entry.interval = EditorHelper.FloatField("Interval (0 = None)", entry.interval);
            entry.removeEffectOnExit = EditorGUILayout.Toggle("Remove On Exit?", entry.removeEffectOnExit);
            EditorGUILayout.Separator();
        }

        if(entry.durationType == EffectZone.EffectZoneDuration.Instant)
        {
            entry.instantZoneLife = EditorHelper.FloatField("Zone Lifetime", entry.instantZoneLife);
        }

        entry.effectZoneImpactVFX = EditorGUILayout.TextField("Impact Effect", entry.effectZoneImpactVFX);
        entry.effectZoneSpawnVFX = EditorGUILayout.TextField("Spawn Effect", entry.effectZoneSpawnVFX);
        entry.size = EditorHelper.EnumPopup("Size", entry.size);
        entry.shape = EditorHelper.EnumPopup("Shape", entry.shape);
        entry.zoneName = EditorGUILayout.TextField("Name", entry.zoneName);
        entry.parentEffectToOrigin = EditorGUILayout.Toggle("Follow Source", entry.parentEffectToOrigin);

        return entry;
    }

    private StatusTypeInfo DrawStatusTypeInfo(StatusTypeInfo entry)
    {
        entry.animBool = EditorGUILayout.TextField("Anim Bool Name", entry.animBool);
        entry.statusType = EditorHelper.EnumPopup("Status Type", entry.statusType);
        entry.stackMethod = EditorHelper.EnumPopup("Stack Method", entry.stackMethod);

        if(entry.stackMethod == Constants.EffectStackingMethod.LimitedStacks)
            entry.maxStacks = EditorGUILayout.IntField("Max Stacks", entry.maxStacks);

        EditorGUILayout.Separator();
        entry.duration = EditorHelper.FloatField("Duration (0 = INF)", entry.duration);
        entry.interval = EditorHelper.FloatField("Interval (0 = None)", entry.interval);

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        entry.vfxName = EditorGUILayout.TextField("VFX Name", entry.vfxName);
        entry.vfxPosOffset = EditorHelper.Vector2Field(entry.vfxPosOffset);
        EditorGUILayout.EndHorizontal();
        entry.onCompleteEffectName = EditorGUILayout.TextField("OnComplete Effect", entry.onCompleteEffectName);


        return entry;
    }

    private StatAdjustmentInfo DrawStatAdjustmentInfo(StatAdjustmentInfo entry)
    {
        entry.targetStat = EditorHelper.EnumPopup("Target Stat", entry.targetStat);
        entry.modType = EditorHelper.EnumPopup("Mod Type", entry.modType);
        entry.adjustmentValue = EditorHelper.FloatField("Value", entry.adjustmentValue);
        entry.permanent = EditorGUILayout.Toggle("Permanent? (Damage / Healing)", entry.permanent);
        entry.options = EditorHelper.DrawList("Base or Cap?", "Option", entry.options, true, StatCollection.StatModifierOption.Base, true, DrawListOfEnums);

        return entry;
    }

    private AddForceInfo DrawAddForceInfo(AddForceInfo entry)
    {
        entry.amount = EditorHelper.FloatField("Amount", entry.amount);
        entry.directionType = EditorHelper.EnumPopup("Direction", entry.directionType);

        if(entry.directionType == AddForceInfo.DirectionType.CustomAngle)
        {
            entry.angle = EditorHelper.FloatField("Angle", entry.angle);
            entry.error = EditorHelper.FloatField("Error", entry.error);
        }
        entry.resetCurrentVelocity = EditorGUILayout.Toggle("Reset Current Velocity?", entry.resetCurrentVelocity);

        return entry;
    }

    private ProjectileInfo DrawProjectileInfo(ProjectileInfo entry)
    {
        entry.spreadType = EditorHelper.EnumPopup("Spread Type", entry.spreadType);
        entry.prefabName = EditorGUILayout.TextField("Prefab Name", entry.prefabName);
        entry.error = EditorHelper.FloatField("Error Range", entry.error);
        entry.projectileCount = EditorGUILayout.IntField("How Many?", entry.projectileCount);

        if (entry.projectileCount > 1)
            entry.burstDelay = EditorHelper.FloatField("Burst Delay", entry.burstDelay);

        entry.addInitialForce = EditorGUILayout.Toggle("Initial Force", entry.addInitialForce);

        if(entry.addInitialForce == true)
        {
            EditorGUILayout.Separator();
            entry.initialForce = DrawAddForceInfo(entry.initialForce);
        }


        return entry;
    }

    private RecoveryData DrawRecoveryData(RecoveryData entry)
    {
        entry.type = EditorHelper.EnumPopup("Recovery Type", entry.type);

        switch (entry.type)
        {
            case Constants.AbilityRecoveryType.Cooldown:
                entry.cooldown = EditorHelper.FloatField("Cooldown", entry.cooldown);
                break;

            case Constants.AbilityRecoveryType.Kills:
                entry.kills = EditorGUILayout.IntField("Kills", entry.kills);
                break;
        }

        return entry;
    }








    private AbilityData DrawAbilityData(List<AbilityData> list, int index)
    {
        AbilityData result = EditorHelper.ObjectField<AbilityData>("Ability", list[index]);
        return result;
    }



    private Constants.AbilityActivationMethod DrawAbilityTriggers(List<Constants.AbilityActivationMethod> list, int index)
    {
        Constants.AbilityActivationMethod result = EditorHelper.EnumPopup("ActivationMethod", list[index]);
        return result;
    }

    private Constants.AbilityActivationCondition DrawActivationConditions(List<Constants.AbilityActivationCondition> list, int index)
    {
        Constants.AbilityActivationCondition result = EditorHelper.EnumPopup("Condition", list[index]);
        return result;
    }


    private T DrawListOfEnums<T>(List<T> list, int index, string label) where T : struct, System.IFormattable, System.IConvertible
    {
        T result = EditorHelper.EnumPopup(label, list[index]);

        return result;
    }

}
