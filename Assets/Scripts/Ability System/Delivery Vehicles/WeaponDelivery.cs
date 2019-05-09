using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDelivery : MonoBehaviour
{

    private Effect parentEffect;

    public AnimHelper AnimHelper { get; private set; }


    private void Awake()
    {

        AnimHelper = GetComponentInChildren<AnimHelper>();
    }

    public void Initialize(Effect parentEffect)
    {
        this.parentEffect = parentEffect;

        AnimHelper.Anim.speed = parentEffect.weaponDeliveryAnimSpeed;
    }


    public void BeginEffectDelivery()
    {
        if (parentEffect == null)
            return;

        parentEffect.BeginDelivery();

    }

    public void CleanUp()
    {
        if (parentEffect == null)
            return;

        parentEffect.Source.Entity().CurrentWeapon = null;
        parentEffect.Source.Entity().WeaponCreated = false;


        Destroy(gameObject);

    }

    public void ReadyNext()
    {
        if (parentEffect == null)
            return;

        parentEffect.Source.Entity().WeaponCreated = false;
    }

}
