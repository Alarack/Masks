using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LL.Events;

public class HealthDeathManager : MonoBehaviour {

    public GameObject corpse;
    public bool cheat;
    public Entity Owner { get; private set; }
    public float Ratio { get { return Owner.EntityStats.GetCappedStatRatio(BaseStat.StatType.Health); } }
    //public float maxHealth;
    public float Health { get {return Owner.EntityStats.GetStatModifiedValue(BaseStat.StatType.Health); } }


    //private float currentHealth;
    private bool dying;


    public void Initialize(Entity owner)
    {
        Owner = owner;
        //currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.StatChanged, OnStatChanged);
    }

    private void OnDisable()
    {
        EventGrid.EventManager.RemoveListener(Constants.GameEvent.StatChanged, OnStatChanged);
    }

    //public void UpdateHealth()
    //{
    //    if (Health <= 0f && dying == false && cheat == false)
    //    {
    //        Die();
    //    }
    //}


    public void OnStatChanged(EventData data)
    {
        GameObject target = data.GetGameObject("Target");
        BaseStat.StatType stat = (BaseStat.StatType)data.GetInt("Stat");
        float value = data.GetFloat("Value");

        //Debug.Log(target.name + " " + stat + " " + value);

        if (target != Owner.gameObject)
            return;

        if (stat != BaseStat.StatType.Health)
            return;

        if (value < 0f)
        {
            Owner.AnimHelper.PlayAnimTrigger("Flinch");
        }

        //Debug.Log(Health + " is the current health of " + Owner.gameObject.name);

        if (Health <= 0f && dying == false && cheat == false)
        {
            Die();
        }

    }

    public void Die()
    {
        dying = true;

        if(LayerMask.LayerToName(Owner.gameObject.layer) == "Enemy")
        {
            GameManager.Instance.spawnManager.EnemyDied(Owner);
            //Debug.Log(gameObject.name + " died");
        }

        Owner.FSMManager.UnregisterEventListeners();
        CreateCorpse();
        Destroy(gameObject);
    }

    private void CreateCorpse()
    {
        if (corpse == null)
            return;

        GameObject activeCorpse = Instantiate(corpse, transform.position, transform.rotation) as GameObject;
        Corpse corpseScript = activeCorpse.GetComponent<Corpse>();
        corpseScript.Initialize();
        corpseScript.PlayDeathEffect();

    }

}
