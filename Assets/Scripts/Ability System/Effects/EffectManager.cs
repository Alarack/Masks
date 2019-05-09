using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectManager {

    public List<Effect> Effects { get { return effects; } }

    [System.NonSerialized]
    private List<Effect> effects = new List<Effect>();
    //public List<StatusEntry> activeEffects = new List<StatusEntry>();

    public Ability ParentAbility { get { return parentAbility; } private set { parentAbility = value; } }

    [System.NonSerialized]
    private Ability parentAbility;

    public EffectManager(Ability ability)
    {
        this.ParentAbility = ability;
    }

    public EffectManager(Ability ability, Effect initialEffect) : this(ability)
    {
        AddEffect(initialEffect);
    }

    public EffectManager(Ability ability, List<Effect> initialEffects) : this (ability)
    {
        effects.AddRange(initialEffects);
    }

    public void RemoveEffectEventListeners()
    {
        int count = effects.Count;
        for (int i = 0; i < count; i++)
        {
            effects[i].RemoveEventListeners();
        }
    }

    public void AddEffect(Effect effect)
    {
        effects.AddUnique(effect);
    }

    public void RemoveEffect(Effect effect)
    {
        effects.RemoveIfContains(effect);
    }

    public void ClearEffects()
    {
        effects.Clear();
    }

    public void SetupRiders()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].SetUpRiders();
        }
    }


    public void ManagedUpdate()
    {
        //int count = effects.Count;
        //for (int i = 0; i < count; i++)
        //{
        //    effects[i].ManagedUpdate();
        //}

        //int statusCount = activeEffects.Count;
        //for (int i = 0; i < statusCount; i++)
        //{
        //    activeEffects[i].ManagedUpdate();
        //}
    }


    #region EFFECT ACTIVATION

    public void ActivateAllEffects()
    {
        int count = effects.Count;
        for (int i = 0; i < count; i++)
        {
            effects[i].Activate();
        }
    }

    public void BeginAllEffectDeliveries()
    {
        int count = effects.Count;

        for (int i = 0; i < count; i++)
        {
            effects[i].BeginDelivery();
        }
    }

    public void DeactivateAllEffects()
    {
        int count = effects.Count;
        for (int i = 0; i < count; i++)
        {
            effects[i].RemoveFromAll();
        }
    }

    public void ActivateSpecificEffect(Effect effect)
    {
        if (effects.Contains(effect))
            effect.Activate();
    }

    public void ActivateSpecificEffect(int effectID)
    {
        Effect targetEffect = GetEffectByID(effectID);
        if (targetEffect != null)
            targetEffect.Activate();
    }

    public void ActivateSpecificEffect(string effectName)
    {
        Effect targetEffect = GetEffectByName(effectName);
        if (targetEffect != null)
            targetEffect.Activate();
    }

    #endregion

    #region EFFECT GETTERS

    public Effect GetEffectByName(string effectName)
    {
        if (string.IsNullOrEmpty(effectName))
        {
            return null;
        }

        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].effectName == effectName)
            {
                return effects[i];
            }
        }

        return null;
    }

    public Effect GetEffectByID(int id)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].EffectID == id)
            {
                return effects[i];
            }
        }

        return null;
    }

    #endregion

    
}
