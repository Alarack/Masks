using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseStateAction {
    public Entity owner;
    public bool RunUpdate { get; protected set; }

    protected List<Ability> abilities = new List<Ability>();
    //protected bool isPlayer;
    protected AIBrain brain;

    public BaseStateAction(Entity owner, bool seperateUpdate = false)
    {
        this.owner = owner;
        RunUpdate = seperateUpdate;

        if(owner is EntityEnemy)
        {
            EntityEnemy enemy = owner as EntityEnemy;
            brain = enemy.Brain;
        }


    }


    public virtual void Execute()
    {
        if (brain != null)
        {
            brain.PushStateActionAbilities(abilities);
        }
        else
        {
            //ActivateAbiliites();
            GetInput();
            //Comment;
        }

    }

    private void GetInput()
    {
        int count = abilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (CheckInput(abilities[i]) == true)
                abilities[i].Activate();
        }
    }

    private bool CheckInput(Ability ability)
    {
        bool manual = ability.GetActivationType(Constants.AbilityActivationMethod.Manual);

        if(manual == true)
        {
            GameInput.GameButtonType targetButton = ability.GetManualActivationButton();

            switch (targetButton)
            {
                case GameInput.GameButtonType.PrimaryAttack:
                    return GameInput.Fire1;
                case GameInput.GameButtonType.SecondaryAttack:
                    return GameInput.Fire2;
                case GameInput.GameButtonType.Jump:
                    return GameInput.Jump;
                case GameInput.GameButtonType.Dash:
                    return GameInput.Dash;
                default:
                    return false;

            }
        }

        return false;
    }

    public virtual void ManagedUpdate()
    {
        //Debug.Log("Managed Update for " + GetType());
        UpdateAbilities();
    }

    public virtual void RegisterEvents()
    {

    }

    public virtual void UnregisterEvents()
    {
        int count = abilities.Count;
        for (int i = 0; i < count; i++)
        {
            abilities[i].EffectManager.RemoveEffectEventListeners();
        }
    }


    public void PopulateAbilities(List<AbilityData> abilityData)
    {
        int count = abilityData.Count;
        for (int i = 0; i < count; i++)
        {
            Ability newAbility = CreateAbility(abilityData[i]);
            abilities.Add(newAbility);
            newAbility.Equip();
        }

        if (abilities.Count > 0)
        {
            //Debug.Log("Turning On Update for " + GetType().ToString());
            RunUpdate = true;
        }

    }


    protected Ability CreateAbility(AbilityData data)
    {
        Ability result = new Ability(data, owner.gameObject, data.sequencedAbilities);
        return result;
    }

    public void ActivateAbiliites()
    {
        int count = abilities.Count;
        for (int i = 0; i < count; i++)
        {
            abilities[i].Activate();
        }
    }

    private void UpdateAbilities()
    {
        //Debug.Log("Updating " + GetType().ToString());

        int count = abilities.Count;
        for (int i = 0; i < count; i++)
        {
            abilities[i].ManagedUpdate();
        }
    }
}
