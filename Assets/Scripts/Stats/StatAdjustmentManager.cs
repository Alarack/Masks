using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LL.Events;
using StatType = BaseStat.StatType;
using StatModificationType = StatModifier.StatModificationType;
using StatModifierOption = StatCollection.StatModifierOption;


public static class StatAdjustmentManager {


    public static void AdjustHealth(StatCollection source, StatCollection target, float value)
    {
        target.ApplyPermanentMod(StatType.Health, value, StatModificationType.Additive, source.Owner);
        SendStatChangeEvent(source.Owner, target.Owner, StatType.Health, value);
    }


    public static void ApplyUntrackedStatMod(StatCollection source, StatCollection target, StatType stat, float value)
    {
        target.ApplyPermanentMod(stat, value, StatModificationType.Additive, source.Owner);
        SendStatChangeEvent(source.Owner, target.Owner, stat, value);
    }

    public static void ApplyUntrackedStatMod(StatCollection source, StatCollection target, StatType stat, float value, StatModificationType modType, params StatModifierOption[] statOptions)
    {
        if (target == null)
            return;

        target.ApplyPermanentMod(stat, value, modType, source.Owner, statOptions);
        SendStatChangeEvent(source.Owner, target.Owner, stat, value);
    }

    //public static void ApplyUntrackedStatMod(StatAdjustment adj) {
    //    adj.target.ApplyUntrackedMod(adj.statType, adj.value, adj.modType, adj.adjustmentOptions);
    //}

    public static StatModifier ApplyTrackedStatMod(StatCollection source, StatCollection target, StatType stat, float value, StatModificationType modType, params StatModifierOption[] statOptions)
    {
        GameObject s = source != null ? source.Owner : null;
        GameObject t = target != null ? target.Owner : null;
        StatModifier mod = target.ApplyAndReturnTrackedMod(stat, value, modType, s, statOptions);

        if (t != null)
        {
            SendStatChangeEvent(s, t, stat, value);
            return mod;
        }
        else
        {
            Debug.LogWarning("a stat mod: " + mod + " could not be added to a target because it was null");
            return null;
        }


    }

    public static void ApplyTrackedStatMod(StatCollection source, StatCollection target, StatType stat, StatModifier mod, params StatModifierOption[] statOptions)
    {
        GameObject s = source != null ? source.Owner : null;
        GameObject t = target != null ? target.Owner : null;

        if(t != null)
        {
            target.ApplyTrackedMod(stat, mod, s, statOptions);
            SendStatChangeEvent(source.Owner, t, stat, mod.Value);
        }
        else
        {
            Debug.LogWarning("a stat mod: " + mod + " could not be added to a target because it was null");
        }

    }

    public static void RemoveTrackedStatMod(StatCollection source, StatCollection target, StatType stat, StatModifier mod, params StatModifierOption[] statOptions)
    {
        GameObject s = source != null ? source.Owner : null;
        GameObject t = target != null ? target.Owner : null;

        if(t != null)
        {
            target.RemoveTrackedMod(stat, mod, s, statOptions);
            SendStatChangeEvent(source.Owner, t, stat, -mod.Value);
        }
        else
        {
            Debug.LogWarning("a stat mod: " + mod + " could not be removed from a target because it was null");
        }

    }





    public static void SendStatChangeEvent(GameObject source, GameObject target, StatType stat, float value)
    {
        EventData data = new EventData();
        data.AddGameObject("Cause", source);
        data.AddGameObject("Target", target);
        data.AddInt("Stat", (int)stat);
        data.AddFloat("Value", value);

        EventGrid.EventManager.SendEvent(Constants.GameEvent.StatChanged, data);

        if(stat == StatType.Health  && target != null)
        {
            VisualEffectLoader.MakeFloatingText(value.ToString(), target.transform.position);

            //Debug.Log(source.name + " has altered " + stat + " on " + target.name + " by " + value);
        }

    }

}
