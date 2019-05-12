using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LL.Events;

public class AnimHelper : MonoBehaviour
{

    public Animator Anim { get; private set; }
    public bool doSlide;
    public bool updateAttackSpeed;
    private Action callback;
    private Entity owner;

    private bool canRecieveEvent = true;

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        owner = GetComponentInParent<Entity>();
    }

    private void Start()
    {

    }

    public void Initialize(Entity owner)
    {
        if (updateAttackSpeed)
            UpdateAttackFloat();
    }

    private void OnEnable()
    {
        if(updateAttackSpeed)
            EventGrid.EventManager.RegisterListener(Constants.GameEvent.StatChanged, OnStatChanged);
    }

    private void OnDisable()
    {
        if (updateAttackSpeed)
            EventGrid.EventManager.RemoveMyListeners(this);
    }


    private void Update()
    {
        if (doSlide)
            HandleSlideAnim();


    }

    private void OnStatChanged(EventData data)
    {
        GameObject target = data.GetGameObject("Target");
        BaseStat.StatType type = (BaseStat.StatType)data.GetInt("Stat");

        if (target != owner.gameObject)
            return;

        if (type != BaseStat.StatType.AttackSpeed)
            return;


        UpdateAttackFloat();
    }

    private void UpdateAttackFloat()
    {
        float attackSpeed = owner.EntityStats.GetStatModifiedValue(BaseStat.StatType.AttackSpeed);

        //Debug.Log(attackSpeed);

        SetFloat("AttackSpeed", attackSpeed);
    }

    private void HandleSlideAnim()
    {
        float xVelocityAbs = Mathf.Abs(owner.Movement.MyPhysics.Velocity.x);

        if (xVelocityAbs > 2f && GetBool("moving") == false)
        {
            PlayOrStopAnimBool("sliding", true);
        }
        else if (xVelocityAbs <= 2f && GetBool("moving") == false)
            PlayOrStopAnimBool("sliding", false);
    }

    public void PlayWalk()
    {
        if (Anim == null)
        {
            return;
        }

        if (Anim.GetBool("moving") == true)
            return;

        Anim.SetBool("moving", true);
    }

    public void StopWalk()
    {
        if (Anim == null)
            return;

        if (Anim.GetBool("moving") == false)
            return;

        Anim.SetBool("moving", false);
    }

    public bool GetBool(string name)
    {
        return Anim.GetBool(name);
    }

    public void PlayOrStopAnimBool(string boolName, bool play = true)
    {
        //Debug.Log("Playing " + boolName + " " + play);
        if (Anim == null)
            return;

        if (Anim.GetBool(boolName) == false && play == false)
            return;

        if (Anim.GetBool(boolName) == true && play == true)
            return;

        Anim.SetBool(boolName, play);
    }

    public void SetFloat(string name, float value)
    {
        Anim.SetFloat(name, value);
    }

    public bool PlayAnimTrigger(string trigger)
    {
        if (Anim == null)
            return false;

        if (string.IsNullOrEmpty(trigger) == true)
            return false;

        try
        {
            Anim.SetTrigger(trigger);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(gameObject.name + " Could not play an animation: " + e);
            return false;
        }
    }

    public void PlayParticleEffect(string particleName)
    {
        ParticleSystem p;

        Transform t = transform.Find("VFX/" +particleName);


        if(t == null)
        {
            Debug.Log("Couldn't find transform " + particleName);
            return;
        }

        p = t.GetComponent<ParticleSystem>();

        if(p == null)
        {
            Debug.Log("Couldn't find particles");
            return;
        }

        p.Play();
        
    }

    public void SetAnimEventAction(Action callback)
    {
        if(this.callback != callback)
            this.callback = callback;
    }

    public void RecieveAnimEvent(AnimationEvent param)
    {
        Debug.Log("Recieving " + param.stringParameter /*+ " from anim event on " + gameObject.name*/);

        if (this.callback != null)
            callback();

        if (canRecieveEvent == false)
        {
            Debug.Log("no");
            return;
        }
        else
        {
            Debug.Log("yes");
        }


        SendEffectDeliveryEvent(param);
    }

    private IEnumerator ResetEvent()
    {
        yield return new WaitForEndOfFrame();
        canRecieveEvent = true;
    }


    private void SendEffectDeliveryEvent(AnimationEvent param)
    {



        //Debug.Log(param.animatorClipInfo.weight);

        if (param.animatorClipInfo.weight < 0.5f)
            return;

        string[] names = param.stringParameter.Split(',');

        if (names.Length < 2)
            return;

        Ability targetAbility = GameManager.GetAbilityByOwner(owner.gameObject, names[0]);

        if(targetAbility == null)
        {
            //Debug.LogError("Anim helper couldn't find an ability with name: " + param.stringParameter);
            return;
        }

        Effect targetEffect = targetAbility.EffectManager.GetEffectByName(names[1]);

        if (targetEffect == null)
        {
            //Debug.LogError("Anim helper couldn't find an effect on " + targetAbility.abilityName + " with name: " + names[1]);
            return;
        }
        canRecieveEvent = false;
        StartCoroutine(ResetEvent());

        Debug.Log("Delivering " + targetEffect.effectName);
        targetEffect.BeginDelivery(targetEffect.weaponDelivery);

    }

}
