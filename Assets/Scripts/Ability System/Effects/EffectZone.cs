using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectZone : MonoBehaviour {

    public enum EffectZoneDuration {
        Instant = 0,
        Persistant = 1,
    }



    public string spawnEffect;
    public string impactEffect;

    public LayerMask LayerMask { get; protected set; }
    public AnimHelper AnimHelper { get; protected set; }

    protected Effect parentEffect;
    protected List<GameObject> targets = new List<GameObject>();


    public virtual void Initialize(Effect parentEffect, LayerMask mask, Transform parentToThis = null)
    {
        this.parentEffect = parentEffect;
        this.LayerMask = mask;
        this.impactEffect = parentEffect.effectZoneInfo.effectZoneImpactVFX;
        this.spawnEffect = parentEffect.effectZoneInfo.effectZoneSpawnVFX;
        AnimHelper = GetComponent<AnimHelper>();

        if(parentToThis != null)
        {
            transform.SetParent(parentToThis, false);
            transform.localPosition = Vector3.zero;
        }

        if(string.IsNullOrEmpty(parentEffect.effectZoneInfo.effectZoneAnimTrigger) == false)
        {
            if(AnimHelper == null)
            {
                AnimHelper helper = gameObject.AddComponent<AnimHelper>();
                helper.PlayAnimTrigger(parentEffect.effectZoneInfo.effectZoneAnimTrigger);
                return;
            }

            AnimHelper.PlayAnimTrigger(parentEffect.effectZoneInfo.effectZoneAnimTrigger);
        }

        //Debug.Log("Effect zone created");

    }

    protected abstract void Apply(GameObject target);
    protected abstract void Remove(GameObject target);

    protected virtual void ApplyToAllTargets()
    {
        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            Apply(targets[i]);
        }
    }

    protected virtual void RemoveAllTargets()
    {
        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            Remove(targets[i]);
        }
    }

    protected virtual void CleanUp()
    {
        Destroy(gameObject);
    }

    protected virtual bool CheckHitTargets(GameObject target)
    {
        if (target == null)
            return false;

        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            if (targets[i] == target)
                return false;
        }

        targets.AddUnique(target);
        return true;
    }

    protected GameObject IsProjectileInTargets(Projectile projectile)
    {
        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            if (targets[i].Projectile() == null)
                continue;

            if (targets[i].Projectile() == projectile)
                return targets[i];
        }

        return null;
    }

    protected GameObject IsEntityInTargets(Entity entity)
    {
        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            if (targets[i].Entity() == null)
                continue;

            if (targets[i].Entity() == entity)
                return targets[i];
        }

        return null;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {

    }

    protected virtual void OnTriggerExit(Collider other)
    {

    }

    protected virtual void OnTriggerStay(Collider other)
    {

    }


    protected virtual void ApplyAfterLayerCheck(GameObject other, bool checkHits = false)
    {
        if (LayerTools.IsLayerInMask(LayerMask, other.layer) == false)
            return;

        if (checkHits == true)
        {
            if (CheckHitTargets(other) == false)
                return;
        }

        Apply(other.gameObject);
    }

    protected void CreateImpactEffect(Vector2 location)
    {
        if (string.IsNullOrEmpty(impactEffect) == true)
        {
            //Debug.Log(parentEffect.effectName + " has no impact effect name. " + impactEffect + " has been given.");
            return;
        }
        

        GameObject loadedPrefab = Resources.Load("HitEffects/" + impactEffect) as GameObject;

        if(loadedPrefab == null)
        {
            Debug.Log("Couldn't load " + impactEffect);
            return;
        }

        //Debug.Log("creating an impact effect vfx clalled " + loadedPrefab.name);

        Vector2 loc = new Vector2(location.x + Random.Range(-0.5f, 0.5f), location.y + Random.Range(-0.5f, 0.5f));

        GameObject impact = Instantiate(loadedPrefab, loc, Quaternion.identity) as GameObject;
        Destroy(impact, 2f);
    }

}
