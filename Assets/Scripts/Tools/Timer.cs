using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer
{

    public float Ratio { get { return _timer / Duration; } }

    public float Duration { get; private set; }
    private bool resetTimerOnComplete;

    private Action completionCallback;

    private float _timer;
    public string timerName;


    public Timer(float duration, Action completionCallback, bool resetOnComplete = false, string timerName = "")
    {
        Duration = duration;
        resetTimerOnComplete = resetOnComplete;
        this.timerName = timerName;

        if (completionCallback != null)
            this.completionCallback += completionCallback;
    }

    public void ModifyDuration(float mod)
    {
        Duration += mod;

        if (Duration <= 0f)
        {
            Duration = 0f;
        }

        if (_timer > Duration)
        {
            _timer = 0f;
        }
    }

    public void SetNewDuration(float duration)
    {
        Duration = duration;

        if (Duration <= 0f)
        {
            Duration = 0f;
        }

        if (_timer > Duration)
        {
            _timer = 0f;
        }
    }

    public void UpdateClock()
    {
        if (_timer < Duration)
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration)
            {
                if (completionCallback != null)
                    completionCallback();

                if (resetTimerOnComplete)
                {
                    ResetTimer();
                }
            }
        }
    }

    public void ResetTimer()
    {
        _timer = 0f;
    }
}

public class ComplexTimer
{
    public Timer duration;
    public Timer interval;

    public bool Complete { get; private set; }

    public ComplexTimer(float totalDuration, float intervalDuration, Action onCompleteAction, Action onIntervalAction)
    {
        onCompleteAction += Finish;

        duration = new Timer(totalDuration, onCompleteAction, false);
        interval = new Timer(intervalDuration, onIntervalAction, true);
    }

    public void RefreshDuration()
    {
        duration.ResetTimer();
    }

    public void ModifyDuration(float mod)
    {
        duration.ModifyDuration(mod);
    }

    public void ModifyInterval(float mod)
    {
        interval.ModifyDuration(mod);
    }

    public void UpdateClocks()
    {
        if (Complete == false)
        {
            //Debug.Log("Updating complex timer");
            duration.UpdateClock();
            interval.UpdateClock();
        }
    }

    private void Finish()
    {
        //Debug.Log("Finishing a complex timer");
        Complete = true;
    }

}
