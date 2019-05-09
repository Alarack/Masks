using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CappedStat : BaseStat {

    public float MaxValue { get{ return maxValue.ModifiedValue; } }
    public float Ratio { get { return GetRatio() /*MaxValue > 0 ? (ModifiedValue / MaxValue) : 0*/; } }


    private BaseStat maxValue;

    public CappedStat(StatType type, float baseValue,  float maxValue) : base(type, baseValue) {
        this.maxValue = new BaseStat(type, maxValue);
    }

    protected float GetRatio()
    {
        if (MaxValue <= 0)
            return 0;


        //Debug.Log(ModifiedValue + " is my mod value");
        //Debug.Log(MaxValue + " is my max value");

        //Debug.Log((ModifiedValue / MaxValue) + " is my ratio");

        return ModifiedValue / MaxValue;
    }

    protected override float GetModifiedValue() {
        float modvalue = base.GetModifiedValue();

        if (modvalue > MaxValue)
            modvalue = MaxValue;

        return modvalue;
    }

    public void ModifyCapPermanently(float value, StatModifier.StatModificationType modType)
    {
        maxValue.ModifyStatPermanently(value, modType);
    }

    public void ModifyCap(float value, StatModifier.StatModificationType modType) {
        maxValue.ModifyStat(value, modType);
    }

    public void ModifyCap(StatModifier mod) {
        maxValue.ModifyStat(mod);
    }

    public void RemoveCapModifier(StatModifier mod) {
        maxValue.RemoveModifier(mod);
    }

    public void ResetCap() {
        maxValue.Reset();
    }

    public void Refresh()
    {
        BaseValue = MaxValue;
    }

}
