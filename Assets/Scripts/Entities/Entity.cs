﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LL.FSM;
using LL.Events;


public enum DimensionMode
{
    Two,
    Three
}


public class Entity : MonoBehaviour
{

    

    public string entityName;

    [Header("Entity Stats")]
    public StatCollectionData statTemplate;


    public AnimHelper AnimHelper { get; protected set; }
    public EntityMovement Movement { get; protected set; }
    public HealthDeathManager Health { get; protected set; }
    public SpriteRenderer SpriteRenderer { get; protected set; }


    public StatCollection EntityStats { get; protected set; }
    public EffectDelivery EffectDelivery { get; protected set; }

    public WeaponDelivery CurrentWeapon { get; set; }
    public bool WeaponCreated { get; set; }

    public FSM EntityFSM { get; protected set; }
    public StateManager FSMManager { get; protected set; }

    protected virtual void Awake()
    {
        GameManager.RegisterEntity(this);

    }

    public virtual void Initialize()
    {
        InitStats();

        SpriteRenderer = GetComponent<SpriteRenderer>();
        AnimHelper = GetComponentInChildren<AnimHelper>();
        if (AnimHelper != null)
            AnimHelper.Initialize(this);
        EffectDelivery = GetComponentInChildren<EffectDelivery>();

        Movement = GetComponent<EntityMovement>();
        if (Movement != null)
            Movement.Initialize(this);


        InitFSM();



        Health = GetComponent<HealthDeathManager>();
        if (Health != null)
            Health.Initialize(this);


    }

    public void InitFSM()
    {
        if(EntityFSM == null)
        {
            EntityFSM = new FSM(this);
        }

        if(FSMManager == null)
        {
            FSMManager = GetComponent<StateManager>();
            if (FSMManager != null)
                FSMManager.Initialize(this, EntityFSM);
        }

    }

    protected virtual void Start()
    {




    }


    protected void InitStats()
    {
        if (EntityStats != null)
            return;

        if (statTemplate == null)
        {
            Debug.LogError(gameObject.name + " does not have a stat template");
            return;
        }

        EntityStats = new StatCollection(gameObject, OnStatChanged, statTemplate);
    }


    private void OnStatChanged(BaseStat.StatType type, GameObject cause)
    {
        //Local Event

        //Global Event
        //EventData data = new EventData();
        //data.AddInt("Type", (int)type);
        //data.AddGameObject("Target", this.gameObject);
        //data.AddGameObject("Cause", cause);
        //EventGrid.EventManager.SendEvent(Constants.GameEvent.StatChanged, data);
    }


}
