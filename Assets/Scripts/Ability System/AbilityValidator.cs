using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LL.Events;

public static class AbilityValidator  {


    public static bool Validate(Ability ability, AbilityActivationInfo activation, EventData eventData = null)
    {
        bool result = false;

        switch (activation.activationMethod)
        {
            case Constants.AbilityActivationMethod.StatChanged:
                result = ValidateOnStatChanged(eventData, activation, ability);
                break;

                
        }


        return result;
    }




    private static bool ValidateOnStatChanged(EventData data, AbilityActivationInfo activator, Ability ability)
    {
        //bool result = false;

        BaseStat.StatType stat = (BaseStat.StatType)data.GetInt("Stat");
        GameObject target = data.GetGameObject("Target");
        //GameObject cause = data.GetGameObject("Cause");
        float value = data.GetFloat("Value");

        if (activator.targetstat != stat)
            return false;

        switch (activator.gainedOrLost)
        {
            case EffectConstraint.GainedOrLost.Gained:
                if (value <= 0)
                    return false;
                break;

            case EffectConstraint.GainedOrLost.Lost:
                if (value >= 0)
                    return false;
                break;
        }

        switch (activator.statChangeTarget)
        {
            case EffectConstraint.TargetConstraintType.Ally:
                if (target.IsSelfOrAlly(ability.Source) == false)
                    return false;
                break;

            case EffectConstraint.TargetConstraintType.Enemy:
                if (target.IsSelfOrAlly(ability.Source) == true)
                    return false;
                break;

            case EffectConstraint.TargetConstraintType.SelfOnly:
                if(target != ability.Source)
                    return false;
                break;
        }





        return true;
    }


}
