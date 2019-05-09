using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpsePart : MonoBehaviour
{
    public enum PartType {
        Head,
        Body,
        Arm,
        Leg,
        Hand
    }

    public PartType partType;
    public float fadeTime;

    private Timer fadeTimer;
    private AnimHelper animHelper;

    private void Awake()
    {
        animHelper = GetComponent<AnimHelper>();
    }

    private void Update()
    {
        if (fadeTimer != null)
            fadeTimer.UpdateClock();
    }

    public void Initialize()
    {
        if(fadeTime > 0f)
            fadeTimer = new Timer(fadeTime, PlayFade);
    }

    private void PlayFade()
    {
        animHelper.SetAnimEventAction(CleanUp);
        animHelper.PlayAnimTrigger("Fade");
    }

    //public void RecieveAnimEvent(AnimationEvent animEvent)
    //{
    //    if (animEvent.stringParameter == "CleanUp")
    //    {
    //        CleanUp();
    //    }
    //}

    public void CleanUp()
    {
        Destroy(gameObject);
    }


}
