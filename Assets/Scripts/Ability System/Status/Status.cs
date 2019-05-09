using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusType = Constants.StatusType;

public class Status  {

    public StatusType statusType;
    public Constants.EffectStackingMethod stackMethod;
    public int StackCount { get; protected set; }
    public int MaxStack { get; protected set; }

    public GameObject Target { get; protected set; }
    public GameObject Source { get; protected set; }
    public Ability SourceAbility { get; protected set; }
    public Effect SourceEffect { get; protected set; }
    public string AnimBoolName { get; protected set; }

    protected Timer durationTimer;
    protected Timer intervalTimer;

    protected Effect onCompleteEffect;

    protected string vfxName;
    protected Vector2 vfxOffset;
    protected GameObject activeVFX;

    public Status(StatusInfo statusInfo)
    {
        SetUp(statusInfo);

        if(statusInfo.typeInfo.duration > 0)
            durationTimer = new Timer(statusInfo.typeInfo.duration, CleanUp, false);

        if(statusInfo.typeInfo.interval > 0)
            intervalTimer = new Timer(statusInfo.typeInfo.interval, Tick, true);
    }

    //public Status(StatusInfo statusInfo, float duration, float interval) : this(statusInfo, duration)
    //{
    //    intervalTimer = new Timer("Interval", interval, true, Tick);
    //}

    protected virtual void SetUp(StatusInfo info)
    {
        statusType = info.typeInfo.statusType;
        MaxStack = info.typeInfo.maxStacks;
        Target = info.targetInfo.target;
        Source = info.targetInfo.source;
        SourceAbility = info.targetInfo.sourceAbility;
        SourceEffect = info.targetInfo.sourceEffect;
        onCompleteEffect = info.GetOnCompleteEffect();
        StackCount = 1;
        stackMethod = info.typeInfo.stackMethod;
        AnimBoolName = info.typeInfo.animBool;
        vfxName = info.typeInfo.vfxName;
        vfxOffset = info.typeInfo.vfxPosOffset;

        //CreateVFX();
    }


    public bool IsFromSameSource(Ability ability)
    {
        return SourceAbility == ability;
    }

    protected virtual void Tick()
    {

    }

    public virtual void FirstApply()
    {
        CreateVFX();
        Tick();
    }

    public virtual void Stack()
    {
        RefreshDuration();
        StackCount++;
    }

    public virtual void Remove()
    {
        CleanUp();
    }

    protected virtual void CleanUp()
    {
        StatusManager.RemoveStatus(Target, this);

        if (activeVFX != null)
        {
            activeVFX.transform.SetParent(null, true);
            ParticleSystem p = activeVFX.GetComponent<ParticleSystem>();
            if (p != null)
                p.Stop();

            GameObject.Destroy(activeVFX, 2f);
        }


        if (onCompleteEffect != null)
        {
            //Debug.Log("Sending On Complete effect");
            onCompleteEffect.Activate();
        }

        
    }


    public virtual void RefreshDuration()
    {
        durationTimer.ResetTimer();
    }

    public virtual void ModifyIntervalTime(float mod)
    {
        intervalTimer.ModifyDuration(mod);
    }

    public virtual void ModifyDuration(float mod)
    {
        durationTimer.ModifyDuration(mod);
    }

    public virtual void ManagedUpdate()
    {
        if(durationTimer != null)
            durationTimer.UpdateClock();

        if(intervalTimer != null)
            intervalTimer.UpdateClock();
    }

    public virtual void CreateVFX()
    {
        if (string.IsNullOrEmpty(vfxName) == true)
            return;

        //if (activeVFX != null)
        //    return;


        GameObject loadedVFX = VisualEffectLoader.LoadVisualEffect("Particles", vfxName);

        if(loadedVFX == null)
        {
            Debug.LogError("Status couldn't load a vfx with name " + vfxName);
            return;
        }


        bool facingLeft = Target.Entity().Movement.Facing == EntityMovement.FacingDirection.Left;
        float xOffset = facingLeft == true ? vfxOffset.x * -1 : vfxOffset.x;

        Vector2 offset = new Vector2(xOffset, vfxOffset.y);

        activeVFX = GameObject.Instantiate(loadedVFX, Target.transform);
        activeVFX.transform.localPosition = Vector3.zero + (Vector3)offset;

    }

}



[System.Serializable]
public struct StatusInfo {
    public StatusTargetInfo targetInfo;
    public StatusTypeInfo typeInfo;

    public StatusInfo(StatusTargetInfo targetInfo, StatusTypeInfo typeInfo)
    {
        this.targetInfo = targetInfo;
        this.typeInfo = typeInfo;
    }


    public Effect GetOnCompleteEffect()
    {
        return targetInfo.sourceAbility.EffectManager.GetEffectByName(typeInfo.onCompleteEffectName);
    }
}


[System.Serializable]
public struct StatusTargetInfo {
    public GameObject target;
    public GameObject source;

    [System.NonSerialized]
    public Ability sourceAbility;

    [System.NonSerialized]
    public Effect sourceEffect;

    public StatusTargetInfo(GameObject target, GameObject source, Ability sourceAbility, Effect sourceEffect)
    {
        this.target = target;
        this.source = source;
        this.sourceAbility = sourceAbility;
        this.sourceEffect = sourceEffect;
    }


}




[System.Serializable]
public struct StatusTypeInfo {

    public Constants.EffectStackingMethod stackMethod;
    public float duration;
    public float interval;
    public string onCompleteEffectName;
    public int maxStacks;
    public StatusType statusType;
    public string animBool;
    public string vfxName;
    public Vector2 vfxPosOffset;

    public StatusTypeInfo(StatusType statusType, float duration, Constants.EffectStackingMethod stackMethod, Vector2 vfxPosOffset, string vfxName = "",
         float interval = 0f, int maxStacks = 1, string onCompleteEffectName = "", string animBool = "")
    {
        this.statusType = statusType;
        this.maxStacks = maxStacks;
        this.onCompleteEffectName = onCompleteEffectName;
        this.duration = duration;
        this.interval = interval;
        this.stackMethod = stackMethod;
        this.animBool = animBool;
        this.vfxName = vfxName;
        this.vfxPosOffset = vfxPosOffset;
    }


}