using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatType = BaseStat.StatType;
using StatModifierOption = StatCollection.StatModifierOption;
using StatModificationType = StatModifier.StatModificationType;

public class StatAdjustment {

    public float value;
    public StatType statType;
    public StatModificationType modType;
    public StatModifierOption[] adjustmentOptions;
    public StatModifier mod;
    public StatCollection source;
    public StatCollection target;


    private StatAdjustment(StatCollection source, StatCollection target, StatType statType, params StatModifierOption[] options) {
        this.source = source;
        this.target = target;
        this.statType = statType;
        this.adjustmentOptions = options;
    }

    public StatAdjustment(StatCollection source, StatCollection target, StatType statType, float value, StatModificationType modType, params StatModifierOption[] options)
        : this(source, target, statType, options) {
        this.value = value;
        this.modType = modType;
    }

    public StatAdjustment(StatCollection source, StatCollection target, StatType statType, StatModifier mod, params StatModifierOption[] options)
        : this(source, target, statType, options){
        this.mod = mod;
    }



}
