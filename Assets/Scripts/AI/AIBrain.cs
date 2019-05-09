using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrain : MonoBehaviour
{
    public EntityEnemy Owner { get; private set; }

    private AISensor sensor;

    public void Initialize(EntityEnemy owner, AISensor sensor)
    {
        this.Owner = owner;
        this.sensor = sensor;
    }

    public void TargetSpotted()
    {
        Owner.Aggro();
    }

    public void NoTargets()
    {
        Owner.EntityFSM.SwapToPreviousState();
    }


    public void PushStateActionAbilities(List<Ability> abilities)
    {
        SelectAbility(abilities);
    }


    private bool IsAbilityInUse(List<Ability> abilities)
    {
        int count = abilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (abilities[i].InUse == true)
                return true;
        }

        return false;
    }

    private void SelectAbility(List<Ability> abilities)
    {

        bool inUse = IsAbilityInUse(abilities);

        Dictionary<Ability, float> weightedAbilityDict = new Dictionary<Ability, float>();

        int count = abilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (sensor.ClosestTarget == null)
            {
                return;
            }

            if (abilities[i].GetActivationType(Constants.AbilityActivationMethod.Manual) == false)
                continue;

            if (abilities[i].MeetsRequiredConditions(sensor.ClosestTarget) == false)
                continue;

            if (abilities[i].RecoveryManager.HasRecovery == true && abilities[i].RecoveryManager.HasCharges == false)
                continue;

            if (inUse && abilities[i].OverrideOtherAbilities == false)
                continue;

            float moddedWeight = abilities[i].GetModifiedWeight(sensor.ClosestTarget);
            weightedAbilityDict.Add(abilities[i], moddedWeight);
        }

        int countOfUsableAbilities = weightedAbilityDict.Count;

        //Debug.Log(countOfUsableAbilities + " abiliites are ready to be used");

        

        if(countOfUsableAbilities > 0)
        {
            Ability chosen = weightedAbilityDict.OrderBy(k => k.Value).Last().Key;

            chosen.Activate();
        }
    }
}
