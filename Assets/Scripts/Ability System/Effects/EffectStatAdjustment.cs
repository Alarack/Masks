using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectStatAdjustment : Effect {

    protected StatAdjustmentInfo adjInfo;
    protected StatCollection sourceStats;
    //private List<StatusStatAdjustment> activeStatus = new List<StatusStatAdjustment>();

    public EffectStatAdjustment(Ability parentAbility, StatAdjustmentInfo adjInfo): base(parentAbility)
    {
        this.adjInfo = adjInfo;
        this.sourceStats = parentAbility.Source.GetStats();
    }

    public override void Apply(GameObject target)
    {
        base.Apply(target);

        switch (durationType)
        {
            case Constants.EffectDurationType.Instant:
                ApplyInstantStatAdjustment(target);
                break;

            case Constants.EffectDurationType.Duration:
            case Constants.EffectDurationType.Periodic:
                CreateAndRegisterStatus(target);
                break;
        }

        //StatAdjustmentManager.ApplyTrackedStatMod(Source.Entity().Stats, target.Entity().Stats, targetStat, mod, options);
    }

    protected void ApplyInstantStatAdjustment(GameObject target)
    {
        StatAdjustmentManager.ApplyUntrackedStatMod(sourceStats, target.GetStats(), adjInfo.targetStat, adjInfo.adjustmentValue, adjInfo.modType, adjInfo.options.ToArray());
    }

    protected override bool CreateAndRegisterStatus(GameObject target)
    {
        StatusStatAdjustment newStatus = new StatusStatAdjustment(statusInfo, adjInfo);
        //Debug.Log(newStatus.statusType + " has been made");

        activeStatus.Add(newStatus);
        StatusManager.AddStatus(target, newStatus);


        return true;
    }


}

[System.Serializable]
public struct StatAdjustmentInfo {
    public float adjustmentValue;
    public BaseStat.StatType targetStat;
    public StatModifier.StatModificationType modType;
    public bool permanent;
    public List<StatCollection.StatModifierOption> options;

    public StatAdjustmentInfo(float adjValue, BaseStat.StatType targetStat, StatModifier.StatModificationType modType, List<StatCollection.StatModifierOption> options, bool permanent)
    {
        this.adjustmentValue = adjValue;
        this.targetStat = targetStat;
        this.modType = modType;
        this.permanent = permanent;
        this.options = options;
    }
}
