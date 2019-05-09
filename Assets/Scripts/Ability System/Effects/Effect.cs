using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LL.Events;
using EffectOrigin = Constants.EffectOrigin;
using EffectDeliveryMethod = Constants.EffectDeliveryMethod;
using EffectTag = Constants.EffectTag;
using EffectType = Constants.EffectType;

[System.Serializable]
public class Effect
{

    public string effectName;
    public string riderTarget;
    public List<EffectTag> tags = new List<EffectTag>();
    public EffectOrigin effectOrigin;


    public bool weaponDelivery;
    public float weaponDeliveryAnimSpeed = 1f;
    public string weaponPrefabName;
    public string weaponAnimTrigger;


    public EffectDeliveryMethod deliveryMethod;
    public EffectType effectType;
    public Constants.EffectDurationType durationType;


    public StatusTypeInfo statusTypeInfo;

    protected StatusTargetInfo statusTargetInfo;
    protected StatusInfo statusInfo;

    //Projectile Stuff
    public ProjectileInfo projectileInfo;



    public ZoneInfo effectZoneInfo;
    public string animationTrigger;
    public LayerMask layerMask;


    public Ability ParentAbility { get { return parentAbility; } protected set { parentAbility = value; } }
    public GameObject Source { get { return ParentAbility.Source; } }
    public List<GameObject> Targets { get; protected set; }
    public int EffectID { get; protected set; }


    [System.NonSerialized]
    protected List<Effect> riders = new List<Effect>();

    [System.NonSerialized]
    protected Ability parentAbility;
    protected EffectZone activeZone;
    protected List<Status> activeStatus = new List<Status>();

    protected bool animStarted;
    protected bool pendingUnregister;

    public Effect()
    {

    }

    public Effect(Ability parentAbility)
    {
        this.ParentAbility = parentAbility;
        Targets = new List<GameObject>();

        EffectID = IDFactory.GenerateEffectID();

    }

    public virtual void SetUpRiders()
    {
        if (deliveryMethod != EffectDeliveryMethod.Rider)
            return;

        Effect host = parentAbility.EffectManager.GetEffectByName(riderTarget);

        if (host != null)
        {
            host.AddRider(this);
        }

    }

    public void RemoveEventListeners()
    {

    }


    public virtual void AddRider(Effect effect)
    {
        riders.AddUnique(effect);
    }

    public virtual void Activate()
    {
        PlayEffectAnim();
    }


    public virtual bool IsFromSameSource(Ability ability)
    {
        return ParentAbility == ability;
    }

    public virtual void BeginDelivery(bool weapon = false)
    {

        if(weapon == true)
        {
            //Debug.Log("Begining delivery with weapon");
            CreateWeapon();
            return;
        }

        //Debug.Log("begining delivery for " + effectName + " on " + parentAbility.abilityName);

        switch (deliveryMethod)
        {
            case EffectDeliveryMethod.Instant:
                activeZone = EffectZoneFactory.CreateEffect(effectZoneInfo, effectOrigin, ParentAbility.Source);

                if (activeZone != null)
                {

                    Transform originPoint = null;
                    if (effectZoneInfo.parentEffectToOrigin == true)
                        originPoint = Source.Entity().EffectDelivery.GetOriginPoint(effectOrigin);

                    //Debug.Log(originPoint + " is the state transform");

                    activeZone.Initialize(this, layerMask, originPoint);
                }


                break;

            case EffectDeliveryMethod.Projectile:
                DeliverProjectiles();
                break;

            case EffectDeliveryMethod.SelfTargeting:
                Apply(Source);
                break;

            case EffectDeliveryMethod.ExistingTargets:
                //foreach (GameObject g in parentAbility.targets)
                //{
                //    Apply(g);
                //}
                break;

            case EffectDeliveryMethod.Rider:

                break;
        }
    }

    #region PROJECTILE CREATION

    protected void DeliverProjectiles()
    {
        //Debug.Log("Delivering projectiles");

        if (projectileInfo.projectileCount == 1)
        {
            Projectile shot = ProjectileFactory.CreateProjectile(projectileInfo, effectOrigin, ParentAbility.Source);
            shot.Initialize(this);

            if (projectileInfo.addInitialForce == true)
            {
                Vector2 force = projectileInfo.initialForce.CalcDirectionAndForce(shot.gameObject, Source);
                shot.GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
        else
        {
            ParentAbility.Source.GetMonoBehaviour().StartCoroutine(CreateProjectileBurst());
        }
    }

    protected IEnumerator CreateProjectileBurst()
    {
        WaitForSeconds delay = new WaitForSeconds(projectileInfo.burstDelay);

        int count = projectileInfo.projectileCount;
        for (int i = 0; i < count; i++)
        {
            Projectile shot = ProjectileFactory.CreateProjectile(projectileInfo, effectOrigin, ParentAbility.Source, i);
            shot.Initialize(this);


            if (projectileInfo.addInitialForce == true)
            {
                Vector2 force = projectileInfo.initialForce.CalcDirectionAndForce(shot.gameObject, Source);
                shot.GetComponent<Rigidbody2D>().AddForce(force);
            }


            if (projectileInfo.burstDelay > 0)
                yield return delay;
            else
                yield return null;

        }
    }

    #endregion

    public virtual void Apply(GameObject target)
    {
        //TODO: Create Effect Constraint Varification System

        if (durationType != Constants.EffectDurationType.Instant)
            CreateStatusInfo(target);


        Targets.AddUnique(target);
        ParentAbility.targets.AddUnique(target);
        //CreateAndRegisterStatus(target);
        ApplyRiderEffects(target);
        SendEffectAppliedEvent(target);


            //Debug.Log("APPLY: " + parentAbility.abilityName + " is applying an effect called " + effectName + " to " + target.name);

    }

    protected virtual void CreateStatusInfo(GameObject target)
    {
        statusTargetInfo = new StatusTargetInfo(target, ParentAbility.Source, ParentAbility, this);
        statusInfo = new StatusInfo(statusTargetInfo, statusTypeInfo);
    }

    public virtual void Remove(GameObject target, GameObject cause = null)
    {
        Targets.RemoveIfContains(target);
        ParentAbility.targets.RemoveIfContains(target);
        RemoveMyActiveStatus();
        SendEffectRemovedEvent(cause, target);
    }


    protected virtual void RemoveMyActiveStatus()
    {
        int count = activeStatus.Count;
        for (int i = 0; i < count; i++)
        {
            activeStatus[i].Remove();
        }

        activeStatus.Clear();
    }

    public virtual void RemoveFromAll()
    {
        int count = Targets.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            Remove(Targets[i]);
        }
    }

    protected virtual void ApplyRiderEffects(GameObject target)
    {
        int count = riders.Count;
        for (int i = 0; i < count; i++)
        {
            riders[i].Apply(target);
        }
    }

    protected virtual bool CreateAndRegisterStatus(GameObject target)
    {
        return true;
    }


    public virtual void PlayEffectAnim()
    {
        //TODO: this assumes the source is always an entity, it could be a projectile
        animStarted = Source.Entity().AnimHelper.PlayAnimTrigger(animationTrigger); // Animation trigger will start the delivery at the right time.

        if (animStarted == false)// Start Delivery Instantly if there isn't an animation.
        {
            //if (Source.Entity() is EntityEnemy)
            //    Debug.Log("anim not found on " + effectName + ", begining delivery immediately for " + effectName);
            BeginDelivery(weaponDelivery);
        }
    }

    public virtual void CreateWeapon()
    {
        if(Source.Entity().WeaponCreated == true)
        {
            //Debug.Log("Weapon already made");

            if (parentAbility.OverrideOtherAbilities)
            {
                Source.Entity().CurrentWeapon.CleanUp();
            }
            else
            {
                InputBuffer.BufferAbility(parentAbility);
                return;
            }
        }

        //Debug.Log("makin' a weapon for " + parentAbility.abilityName);

        GameObject loadedPrefab = VisualEffectLoader.LoadVisualEffect("Weapons", weaponPrefabName);
        if(loadedPrefab == null)
        {
            Debug.LogError("Couldn't Load " + weaponPrefabName);
            return;
        }

        Transform point = Source.Entity().EffectDelivery.GetOriginPoint(effectOrigin);

        GameObject activeWeapon = GameObject.Instantiate(loadedPrefab, point);

        activeWeapon.transform.SetParent(point, false);
        activeWeapon.transform.localPosition = Vector3.zero;

        if (Source.Entity().Movement.Facing == EntityMovement.FacingDirection.Left)
            activeWeapon.transform.localScale = new Vector3(
                activeWeapon.transform.localScale.x * -1,
                activeWeapon.transform.localScale.y,
                activeWeapon.transform.localScale.z);

        WeaponDelivery delivery = activeWeapon.GetComponentInChildren<WeaponDelivery>();
        delivery.AnimHelper.PlayAnimTrigger(weaponAnimTrigger);
        Source.Entity().CurrentWeapon = delivery;
        Source.Entity().WeaponCreated = true;
        delivery.Initialize(this);

    }


    #region EVENTS
    protected void SendEffectAppliedEvent(GameObject target)
    {
        EventData data = new EventData();
        data.AddGameObject("Cause", Source);
        data.AddGameObject("Target", target);
        data.AddEffect("Effect", this);

        EventGrid.EventManager.SendEvent(Constants.GameEvent.EffectApplied, data);
    }

    protected void SendEffectRemovedEvent(GameObject cause, GameObject target)
    {
        EventData data = new EventData();
        data.AddGameObject("Cause", cause);
        data.AddGameObject("Target", target);
        data.AddEffect("Effect", this);

        EventGrid.EventManager.SendEvent(Constants.GameEvent.EffectRemoved, data);
    }


    #endregion


}


[System.Serializable]
public struct EffectOriginPoint
{
    public Transform point;
    public Constants.EffectOrigin originType;
}


[System.Serializable]
public struct ZoneInfo
{
    public VisualEffectLoader.VisualEffectShape shape;
    public VisualEffectLoader.VisualEffectSize size;
    public EffectZone.EffectZoneDuration durationType;
    public float instantZoneLife;
    public float duration;
    public float interval;
    public bool removeEffectOnExit;
    public bool parentEffectToOrigin;
    public string effectZoneImpactVFX;
    public string effectZoneSpawnVFX;
    public string zoneName;


    public ZoneInfo(VisualEffectLoader.VisualEffectShape shape, VisualEffectLoader.VisualEffectSize size, EffectZone.EffectZoneDuration durationType,
        float duration, float interval, bool removeEffectOnExit, bool parentEffectToOrigin, string effectZoneImpactVFX, string effectZoneSpawnVFX,
        string zoneName, float instantZoneLife)
    {
        this.shape = shape;
        this.size = size;
        this.durationType = durationType;
        this.duration = duration;
        this.interval = interval;
        this.removeEffectOnExit = removeEffectOnExit;
        this.parentEffectToOrigin = parentEffectToOrigin;
        this.effectZoneImpactVFX = effectZoneImpactVFX;
        this.effectZoneSpawnVFX = effectZoneSpawnVFX;
        this.zoneName = zoneName;
        this.instantZoneLife = instantZoneLife;
    }

}
