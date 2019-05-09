using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {

    public List<AbilityData> defaultAbilities = new List<AbilityData>();
    public bool updateHotbar;

    public GameObject Owner { get; protected set; }

    protected Dictionary<int, Ability> slottedAbilities = new Dictionary<int, Ability>();
    protected List<Ability> unslottedAbiliites = new List<Ability>();


    public void Initialize(GameObject owner)
    {
        this.Owner = owner;

        CreateDefaultAbilities();
    }

    private void Update()
    {
        int count = slottedAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            slottedAbilities[i].ManagedUpdate();
        }
    }


    #region ABILITY CREATION

    private void CreateDefaultAbilities()
    {
        int count = defaultAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            CreateAndAddAbility(defaultAbilities[i]);
            SlotAbility(unslottedAbiliites[i], i);
        }
    }

    public Ability CreateAbility(AbilityData data)
    {
        Ability newAbility = new Ability(data, Owner);

        return newAbility;
    }

    public void CreateAndAddAbility(AbilityData data)
    {
        unslottedAbiliites.Add(CreateAbility(data));
    }

    #endregion

    #region SLOTTING AND UNSLOTTING

    public void AddAbility(Ability ability)
    {
        unslottedAbiliites.AddUnique(ability);
    }

    public void SlotAbility(Ability ability, int slot)
    {
        if(slot > 7)
        {
            Debug.LogError("There are only 8 ability slots");
            return;
        }

        Ability existingAbility = null;

        slottedAbilities.TryGetValue(slot, out existingAbility);

        if (existingAbility != null)
        {
            UnequipAbility(slot, existingAbility);
        }

        EquipAbility(slot, ability);
    }

    public void UnslotAbility(Ability ability)
    {
        KeyValuePair<int, Ability> target = new KeyValuePair<int, Ability>();

        int count = slottedAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            if(slottedAbilities[i] == ability)
            {
                target = new KeyValuePair<int, Ability>(i, slottedAbilities[i]);
                break;
            }
        }

        if(target.Value != null)
        {
            UnequipAbility(target.Key, target.Value);
        }
    }

    public void UnslotAbility(int index)
    {
        Ability target = null;
        slottedAbilities.TryGetValue(index, out target);

        if(target != null)
        {
            UnequipAbility(index, target);
        }
    }


    protected void EquipAbility(int slot, Ability ability)
    {
        slottedAbilities.Add(slot, ability);
        unslottedAbiliites.RemoveIfContains(ability);
        ability.Equip();

        if (updateHotbar)
        {
            //HUD.SetQuickBarSlot(ability, slot);
        }
    }

    protected void UnequipAbility(int slot, Ability ability)
    {
        ability.Unequip();
        slottedAbilities.Remove(slot);
        unslottedAbiliites.AddUnique(ability);

        if (updateHotbar)
        {
            //HUD.SetQuickBarSlot(null, slot);
        }
    }

    #endregion

    #region ACTIVATION

    public void ActivateAbility(int abilityIndex)
    {
        Ability target = null;
        slottedAbilities.TryGetValue(abilityIndex, out target);

        if(target != null)
        {
            if (target.OverrideOtherAbilities)
                target.Activate();
            else if (IsAbilityInUse() == false)
                target.Activate();
        }
            
    }

    public bool IsAbilityInUse()
    {
        int count = slottedAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (slottedAbilities[i].InUse)
                return true;
        }

        return false;
    }

    #endregion



    #region GETTERS

    public Ability GetAbility(string abilityName)
    {
        List<Ability> allAbilities = GetAllAbilities();
        int count = allAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (allAbilities[i].abilityName == abilityName)
                return allAbilities[i];
        }

        return null;
    }

    public Ability GetAbility(int id)
    {
        List<Ability> allAbilities = GetAllAbilities();
        int count = allAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (allAbilities[i].AbilityID == id)
                return allAbilities[i];
        }

        return null;
    }

    protected List<Ability> GetAllAbilities()
    {
        List<Ability> allAbilities = new List<Ability>();
        //int countOfSlotted = slottedAbilities.Count;

        //for (int i = 0; i < countOfSlotted; i++)
        //{
        //    allAbilities.Add(slottedAbilities[i]);
        //}
        allAbilities.AddRange(slottedAbilities.Values);
        allAbilities.AddRange(unslottedAbiliites);

        return allAbilities;

    }

    #endregion
}
