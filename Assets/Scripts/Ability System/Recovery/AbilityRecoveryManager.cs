using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class AbilityRecoveryManager {

    [System.NonSerialized]
    public StatCollection RecoveryStats;

    [System.NonSerialized]
    private List<AbilityRecovery> recoveryMethods = new List<AbilityRecovery>();


    public Ability ParentAbility { get { return parentAbility; } private set { parentAbility = value; } }
    //public bool IsReady { get { return IsAnyRecoveryReady(); } }
    public float Ratio { get { return GetLowestRatio(); } }
    public int Charges { get { return (int)RecoveryStats.GetStatModifiedValue(BaseStat.StatType.AbilityCharge); } }
    public int MaxCharges { get { return (int)RecoveryStats.GetStatMaxValue(BaseStat.StatType.AbilityCharge); } }
    public bool HasCharges { get { return Charges > 0; } }
    public bool HasRecovery { get { return recoveryMethods.Count > 0; } }

    [System.NonSerialized]
    private Ability parentAbility;

    //private List<StatModifier> maxCapMods = new List<StatModifier>();
    //private StatModifier recoveryMod;

    public AbilityRecoveryManager(Ability parentAbility)
    {
        //maxCapMod = new StatModifier(1, StatModifier.StatModificationType.Additive);
        //recoveryMod = new StatModifier(-1, StatModifier.StatModificationType.Additive);
        //spendMod = new StatModifier(-1)

        this.ParentAbility = parentAbility;
        RecoveryStats = new StatCollection(parentAbility.Source, OnStatChanged);
        RecoveryStats.AddStat(BaseStat.StatType.AbilityCharge, 1, 1);

    }

    public void ManagedUpdate()
    {
        int count = recoveryMethods.Count;
        for (int i = 0; i < count; i++)
        {
            recoveryMethods[i].ManagedUpdate();
        }
    }

    public void AddRecoveryMethod(AbilityRecovery recoveryMethod)
    {
        int count = recoveryMethods.Count;
        for (int i = 0; i < count; i++)
        {
            if(recoveryMethods[i].type == recoveryMethod.type)
            {
                Debug.LogError(ParentAbility.abilityName + " already has a recovery method of type: " + recoveryMethod.type);
                return;
            }
        }

        recoveryMethods.Add(recoveryMethod);
    }

    public void RemoveRecoveryMethod(AbilityRecovery recoveryMethod)
    {
        recoveryMethods.RemoveIfContains(recoveryMethod);
    }


    #region CHARGES

    public void RecoverCharge()
    {
        StatAdjustmentManager.ApplyUntrackedStatMod(RecoveryStats, RecoveryStats, BaseStat.StatType.AbilityCharge, 1, StatModifier.StatModificationType.Additive);
    }

    public void SpendCharge()
    {
        StatAdjustmentManager.ApplyUntrackedStatMod(RecoveryStats, RecoveryStats, BaseStat.StatType.AbilityCharge, -1f, StatModifier.StatModificationType.Additive);
    }

    public void AddMaxCharge(StatCollection source, StatModifier mod)
    {
        StatAdjustmentManager.ApplyTrackedStatMod(source, RecoveryStats, BaseStat.StatType.AbilityCharge, mod, StatCollection.StatModifierOption.Cap);
    }

    public void RemoveMaxCharge(StatCollection source, StatModifier mod)
    {
        StatAdjustmentManager.RemoveTrackedStatMod(source, RecoveryStats, BaseStat.StatType.AbilityCharge, mod, StatCollection.StatModifierOption.Cap);
    }


    #endregion

    //public bool IsAnyRecoveryReady()
    //{
    //    int count = recoveryMethods.Count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (recoveryMethods[i].IsReady)
    //            return true;
    //    }

    //    return false;
    //}

    public float GetLowestRatio()
    {
        List<float> allRatios = new List<float>();

        int count = recoveryMethods.Count;
        for (int i = 0; i < count; i++)
        {
            allRatios.Add(recoveryMethods[i].GetRatio());
        }
        
        if(allRatios.Count > 0)
        {
            return allRatios.Min();
        }

        return 0f;

    }


    private void OnStatChanged(BaseStat.StatType type, GameObject cause)
    {

    }

}
