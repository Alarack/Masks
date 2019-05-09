using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BaseStat {

    public enum StatVariant {
        None = 0,
        Simple = 1,
        Capped = 2,
    }



    public enum StatType {
        None = 0,
        Health = 1,
        BaseDamage = 2,
        CritChance = 3,
        CritMultiplier = 4,
        AttackSpeed = 5,
        MoveSpeed = 6,
        Lifetime = 7,
        DamageReduction = 8,
        RotateSpeed = 9,
        CoolDown = 10,
        AbilityCharge = 11,
        ProjectilePenetration = 12,

    }

    public StatType Type { get; protected set; }
    public float BaseValue { get; protected set; }
    public float ModifiedValue { get { return GetModifiedValue(); } }

    protected List<StatModifier> mods = new List<StatModifier>();

    public BaseStat(StatType type, float baseValue)
    {
        Type = type;
        BaseValue = baseValue;
    }

    public virtual void ModifyStat(StatModifier mod)
    {
        mods.Add(mod);
    }

    public virtual void ModifyStat(StatModifier mod, ref float update)
    {
        ModifyStat(mod);
        update = ModifiedValue;
    }

    public virtual void ModifyStatPermanently(float value, StatModifier.StatModificationType modType)
    {
        switch (modType)
        {
            case StatModifier.StatModificationType.Additive:
                BaseValue += value;
                break;

            case StatModifier.StatModificationType.Multiplicative:
                float adj;

                if (value < 0)
                {
                    adj = Mathf.Clamp01(1 + value);
                }
                else
                {
                    adj = 1 + value;
                }

                BaseValue *= adj;

                break;
        }

        if (this is CappedStat)
        {
            CappedStat stat = this as CappedStat;

            if (stat.BaseValue > stat.MaxValue)
            {
                stat.Refresh();
            }
        }

    }

    public virtual StatModifier ModifyStatAndReturnMod(float value, StatModifier.StatModificationType modType)
    {
        StatModifier newMod = new StatModifier(value, modType);
        ModifyStat(newMod);
        return newMod;
    }

    public virtual void ModifyStat(float value, StatModifier.StatModificationType modType)
    {
        ModifyStat(new StatModifier(value, modType));
    }

    public virtual void ModifyStat(float value, StatModifier.StatModificationType modType, ref float update)
    {
        ModifyStat(new StatModifier(value, modType), ref update);
    }

    public virtual void RemoveModifier(StatModifier mod)
    {
        if (mods.Contains(mod))
        {
            mods.Remove(mod);
        }

    }

    public virtual void Reset()
    {
        mods.Clear();
    }

    protected virtual float GetModifiedValue()
    {
        float result = BaseValue + GetTotalAdditiveMod();
        result *= GetTotalMultiplier();

        return result;
    }

    protected float GetTotalMultiplier()
    {
        float totalMultiplier = (1 + GetTotalModByType(StatModifier.StatModificationType.Multiplicative));


        //if (Type == StatType.MoveSpeed)
        //{
        //    Debug.Log(Type + " has a multiplyer of " + totalMultiplier);
        //}

        if (totalMultiplier < 0f)
            totalMultiplier = 0f;

        return totalMultiplier;
    }

    protected float GetTotalAdditiveMod()
    {
        return GetTotalModByType(StatModifier.StatModificationType.Additive);
    }


    protected float GetTotalModByType(StatModifier.StatModificationType type)
    {
        float total = 0f;

        int count = mods.Count;

        for (int i = 0; i < count; i++)
        {
            total += mods[i].GetValueByModType(type);
        }


        if (Type == StatType.MoveSpeed && type == StatModifier.StatModificationType.Multiplicative)
        {
            //Debug.Log(mods.Count + " mods found");
            //Debug.Log(Type + " has a multiplyer of " + total);
        }

        return total;
    }

}
