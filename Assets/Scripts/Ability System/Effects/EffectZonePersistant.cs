using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectZonePersistant : EffectZone {

    public bool removeEffectOnExit;
    //public string deathEffect;
    private ComplexTimer complexTimer;
    private Timer timer;


    public void InitializePersistantZone(float duration, float interval = -1f, bool removeEffectOnExit = false)
    {
        this.removeEffectOnExit = removeEffectOnExit;

        if(interval < 0)
        {
            SetDuration(duration);
        }
        else
        {
            SetDuration(duration, interval);
        }
    }

    private void SetDuration(float duration, float interval)
    {
        complexTimer = new ComplexTimer(duration, interval, OnTimerComplete, OnInterval);
    }

    private void SetDuration(float duration)
    {
        timer = new Timer(duration, OnTimerComplete, false);
    }


    private void Update()
    {
        if (complexTimer != null)
        {
            complexTimer.UpdateClocks();
        }

        if (timer != null)
            timer.UpdateClock();
    }


    protected override void OnTriggerStay(Collider other)
    {
        ApplyAfterLayerCheck(other.gameObject);

        //if (LayerTools.IsLayerInMask(LayerMask, other.gameObject.layer) == false)
        //    return;

        //if (CheckHitTargets(other.gameObject) == true)
        //    Apply(other.gameObject);

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ApplyAfterLayerCheck(other.gameObject);

        //if (LayerTools.IsLayerInMask(LayerMask, other.gameObject.layer) == false)
        //    return;

        //if (CheckHitTargets(other.gameObject) == true)
        //    Apply(other.gameObject);
    }

    protected override void OnTriggerExit(Collider other)
    {
        Remove(other.gameObject);

        //if (targets.Contains(other.gameObject) == true)
        //{
        //    Remove(other.gameObject);

        //    if (removeEffectOnExit)
        //        parentEffect.Remove(other.gameObject);
        //}

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Remove(other.gameObject);

        //if (targets.Contains(other.gameObject) == true)
        //{
        //    Remove(other.gameObject);

        //    if (removeEffectOnExit)
        //        parentEffect.Remove(other.gameObject);
        //}

    }

    protected override void Apply(GameObject target)
    {
        parentEffect.Apply(target);

        CreateImpactEffect(target.transform.position);

    }

    protected override void Remove(GameObject target)
    {
        if (targets.RemoveIfContains(target))
        {
            if (removeEffectOnExit)
                parentEffect.Remove(target);
        }
    }

    protected override void CleanUp()
    {
        RemoveAllTargets();
        base.CleanUp();

    }

    private void OnTimerComplete()
    {
        CleanUp();
    }

    private void OnInterval()
    {
        ApplyToAllTargets();
    }
}
