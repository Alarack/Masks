using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StatType = BaseStat.StatType;
using StatModificationType = StatModifier.StatModificationType;

public class StatCollection {

    public enum StatModifierOption {
        Base,
        Cap,
    }

    public GameObject Owner { get; private set; }

    private List<BaseStat> stats = new List<BaseStat>();
    private Action<StatType, GameObject> onStatChanged;
    //private Dictionary<StatType, Action<StatType, float>> statModifierCallbackDick = new Dictionary<StatType, Action<StatType, float>>();

    public StatCollection(GameObject owner, Action<StatType, GameObject> onStatChanged) {
        this.Owner = owner;
        if(onStatChanged != null) {
            this.onStatChanged += onStatChanged;
        }

    }

    public StatCollection(GameObject owner, Action<StatType, GameObject> onStatChanged, List<BaseStat> defaultStats) : this(owner, onStatChanged) {
        for (int i = 0; i < defaultStats.Count; i++) {
            AddStat(defaultStats[i]);
        }
    }

    public StatCollection(GameObject owner, Action<StatType, GameObject> onStatChanged, StatCollectionData defaultStats) : this(owner, onStatChanged)
    {
        defaultStats.CreateStatsFromData(this);
    }

    public void AddStat(BaseStat stat) {
        if (IsStatAlreadyPresent(stat.Type)) {
            Debug.LogError("Duplicate Stat");
            return;
        }

        stats.Add(stat);
    }

    public void AddStat(StatType type, float value) {
        BaseStat newStat = new BaseStat(type, value);
        AddStat(newStat);
    }

    public void AddStat(StatType type, float value, float maxValue) {
        CappedStat newCappedStat = new CappedStat(type, value, maxValue);
        AddStat(newCappedStat);
    }

    public void RemoveStat(StatType type) {
        BaseStat targetStat = GetStat(type);

        if(targetStat == null) {
            Debug.Log("Stat: " + type + " not found");
            return;
        }

        RemoveStat(targetStat);
    }

    private void RemoveStat(BaseStat stat) {
        if (stats.Contains(stat)) {
            stats.Remove(stat);
        }
    }




    #region Get Stat Values

    public float GetStatModifiedValue(StatType type) {
        BaseStat targetStat = GetStat(type);

        return targetStat != null ? targetStat.ModifiedValue : 0f;
    }

    public float GetStatBaseValue(StatType type) {
        BaseStat targetStat = GetStat(type);

        return targetStat != null ? targetStat.BaseValue : 0f;
    }

    public float GetStatMaxValue(StatType type) {
        BaseStat targetStat = GetStat(type);

        CappedStat cap = TryConvertToCappedStat(targetStat);

        if (cap != null)
            return cap.MaxValue;

        //if(targetStat != null && targetStat is CappedStat) {
        //    CappedStat capped = targetStat as CappedStat;

        //    return capped.MaxValue;
        //}

        Debug.LogError("The maximum value was reqested for a stat with no max. Returning 0");

        return 0f;
    }

    public float GetCappedStatRatio(StatType type) {
        BaseStat targetStat = GetStat(type);

        //Debug.Log("trying to get the stat ratio for " + type);

        CappedStat cap = TryConvertToCappedStat(targetStat);

        if (cap != null)
        {
            //Debug.Log(type + " has a ratio of " + cap.Ratio);

            return cap.Ratio;
        }



        //if (targetStat != null && targetStat is CappedStat) {
        //    CappedStat capped = targetStat as CappedStat;

        //    return capped.Ratio;
        //}

        Debug.LogError("The ratio value was reqested for a stat with no max. Returning 0");

        return 0f;
    }


    #endregion



    #region Modify Stats

    public void ApplyPermanentMod(StatType type, float value, StatModificationType modType, GameObject cause, params StatModifierOption[] statOptions) {
        List<StatModifierOption> options = ConvertStatOptionsToList(statOptions);

        BaseStat targetStat = GetStat(type);
        if(targetStat == null) {
            Debug.LogError("Stat: " + type + " not found");
            return;
        }

        if (options.Contains(StatModifierOption.Cap)){
            CappedStat capped = TryConvertToCappedStat(targetStat);

            if (capped != null) {
                capped.ModifyCapPermanently(value, modType);
                OnStatChanged(type, cause);
            }

        }

        if ((options.Count < 1 || options.Contains(StatModifierOption.Base))) {
            targetStat.ModifyStatPermanently(value, modType);
            OnStatChanged(type, cause);
        }

    }
    public StatModifier ApplyAndReturnTrackedMod(StatType type, float value, StatModificationType modType, GameObject cause, params StatModifierOption[] statOptions)
    {
        StatModifier newMod = new StatModifier(value, modType);
        ApplyTrackedMod(type, newMod, cause, statOptions);
        return newMod;
    }

    public void ApplyTrackedMod(StatType type, StatModifier mod, GameObject cause, params StatModifierOption[] statOptions) {
        List<StatModifierOption> options = ConvertStatOptionsToList(statOptions);

        BaseStat targetStat = GetStat(type);
        if (targetStat == null) {
            Debug.Log("Stat: " + type + " not found");
            return;
        }

        if (options.Contains(StatModifierOption.Cap)) {
            CappedStat capped = TryConvertToCappedStat(targetStat);

            if (capped != null) {
                capped.ModifyCap(mod);
                OnStatChanged(type, cause);
            }

        }

        if ((options.Count < 1 || options.Contains(StatModifierOption.Base))) {
            targetStat.ModifyStat(mod);
            OnStatChanged(type, cause);
        }

    }

    public void RemoveTrackedMod(StatType type, StatModifier mod, GameObject cause, params StatModifierOption[] statOptions) {
        List<StatModifierOption> options = ConvertStatOptionsToList(statOptions);

        BaseStat targetStat = GetStat(type);
        if (targetStat == null) {
            Debug.Log("Stat: " + type + " not found");
            return;
        }

        if (options.Contains(StatModifierOption.Cap)) {
            CappedStat capped = TryConvertToCappedStat(targetStat);

            if (capped != null) {
                capped.RemoveCapModifier(mod);
                OnStatChanged(type, cause);
            }
        }

        if ((options.Count < 1 || options.Contains(StatModifierOption.Base))) {
            targetStat.RemoveModifier(mod);
            OnStatChanged(type, cause);
        }
    }

    public void ResetStat(StatType type, params StatModifierOption[] statOptions) {
        List<StatModifierOption> options = ConvertStatOptionsToList(statOptions);

        BaseStat targetStat = GetStat(type);
        if (targetStat == null) {
            Debug.Log("Stat: " + type + " not found");
            return;
        }

        if (options.Contains(StatModifierOption.Cap)) {
            CappedStat capped = TryConvertToCappedStat(targetStat);

            if (capped != null) {
                capped.ResetCap();
                OnStatChanged(type, null);
            }
        }

        if ((options.Count < 1 || options.Contains(StatModifierOption.Base))) {
            targetStat.Reset();
            OnStatChanged(type, null);
        }
    }


    private void OnStatChanged(StatType type, GameObject cause) {
        if (onStatChanged != null)
            onStatChanged(type, cause);
    }




    #endregion

    private BaseStat GetStat(StatType type) {
        int count = stats.Count;

        for (int i = 0; i < count; i++) {
            if (stats[i].Type == type) {
                return stats[i];
            }
        }

        return null;
    }

    private CappedStat TryConvertToCappedStat(BaseStat targetStat) {
        CappedStat capped = null;

        //Debug.Log(targetStat.Type + " is being converted");

        try {
            capped = targetStat as CappedStat;
            if(capped == null) {
                throw new System.NullReferenceException();
            }
        }
        catch {
            Debug.LogError(" Stat collection tried to convert a stat to a capped stat but failed");
            return null;
        }

        return capped;
    }

    private bool IsStatAlreadyPresent(StatType type) {
        BaseStat targetStat = GetStat(type);

        return targetStat != null ? true : false;
    }


    private List<StatModifierOption> ConvertStatOptionsToList(StatModifierOption[] options) {
        List<StatModifierOption> results = new List<StatModifierOption>();

        for (int i = 0; i < options.Length; i++) {
            results.Add(options[i]);
        }

        return results;
    }



}
