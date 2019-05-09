using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectConstraint {

    public enum EffectConstraintType {
        None = 0,
        Status = 1,
        Type = 2,
        Stat = 3,
    }


    public enum TargetConstraintType {
        None = 0,
        Ally = 1,
        Enemy = 2,
        Either = 3,
        SelfOnly = 4,
    }

    public enum GainedOrLost {
        Gained,
        Lost
    }

    public enum MoreOrLess {
        AtLeast,
        NoMoreThan
    }


    public List<EffectConstraintType> types = new List<EffectConstraintType>();

    public List<TargetConstraintType> targets = new List<TargetConstraintType>();

    public List<Constants.StatusType> statuses = new List<Constants.StatusType>();




}



[System.Serializable]
public class EffectTargetConstraint {

}

[System.Serializable]
public class EffectStatusConstraint {

}


[System.Serializable]
public class EffectStatConstraint {
    public BaseStat.StatType stat;
    public EffectConstraint.MoreOrLess moreOrLess;


    public bool IsStatHigherThan(float percentage, float statValue, float statMaxValue)
    {
        return percentage < (statValue / statMaxValue);
    }

    public bool IsStatAtleast(float percentage, float statValue, float statMaxValue)
    {
        return percentage >= (statValue / statMaxValue);
    }
}
