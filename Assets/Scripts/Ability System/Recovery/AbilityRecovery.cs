using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbilityRecovery {

    public Constants.AbilityRecoveryType type;

    public Ability ParentAbility { get { return parentAbility; } protected set { parentAbility = value; } }
    //public bool IsReady { get; protected set; }

    [System.NonSerialized]
    protected AbilityRecoveryManager manager;

    [System.NonSerialized]
    protected Ability parentAbility;


    public AbilityRecovery()
    {
        //IsReady = true;
    }

    public AbilityRecovery(Ability parentAbility, AbilityRecoveryManager manager)
    {
        //IsReady = true;
        this.ParentAbility = parentAbility;
        this.manager = manager;
    }



    public virtual void BeginRecovery()
    {
        //IsReady = false;
    }

    public virtual void Refresh()
    {
        manager.RecoverCharge();
        //IsReady = true;

        //Debug.Log("Refreshing a charge");
    }

    public abstract void Recover();
    public abstract float GetRatio();
    public abstract void ManagedUpdate();




}
