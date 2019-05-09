using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryCooldown : AbilityRecovery {

    //public float cooldown;

    public float CoolDown { get { return Stats.GetStatModifiedValue(BaseStat.StatType.CoolDown); } }

    private StatCollection Stats;
    private Timer coolDownTimer;


    public RecoveryCooldown(float cooldown, Ability parentAbility, AbilityRecoveryManager manager) : base(parentAbility, manager)
    {
        Stats = new StatCollection(ParentAbility.Source, OnStatChanged);
        Stats.AddStat(BaseStat.StatType.CoolDown, cooldown);

        coolDownTimer = new Timer(cooldown, Refresh, true);
    }



    public override float GetRatio()
    {
        return coolDownTimer.Ratio;
    }

    public override void Recover()
    {
        if (manager.Charges < manager.MaxCharges)
        {
            //Debug.Log(manager.Charges + " are my charges");

            coolDownTimer.UpdateClock();
        }

    }

    public override void ManagedUpdate()
    {
        Recover();
    }


    public void ModifyCoolDown(StatCollection cause, StatModifier mod)
    {
        StatAdjustmentManager.ApplyTrackedStatMod(cause, Stats, BaseStat.StatType.CoolDown, mod);
    }


    private void OnStatChanged(BaseStat.StatType type, GameObject source)
    {
        if (type == BaseStat.StatType.CoolDown)
        {
            coolDownTimer.SetNewDuration(Stats.GetStatModifiedValue(BaseStat.StatType.CoolDown));
        }
    }
}
