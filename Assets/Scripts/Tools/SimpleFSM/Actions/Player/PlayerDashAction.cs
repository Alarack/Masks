using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAction : BaseStateAction {

    //private Timer dashTimer;
    //private Timer dashCooldownTimer;
    //private bool usedDash;

    //private PlayerController playerController;
    //private StatModifier mod;

    public PlayerDashAction(Entity owner, bool runUpdate):base(owner, runUpdate)
    {
        //playerController = owner.Movement as PlayerController;
        //usedDash = false;

        //mod = new StatModifier(20f, StatModifier.StatModificationType.Additive);

        //dashTimer = new Timer(playerController.dashDuration, EndDash, true);
        //dashCooldownTimer = new Timer(playerController.dashCooldown, RefreshDash, true);
    }


    public override void Execute()
    {
        if (GameInput.Dash)
        {
            //BeginDash();
            ActivateAbiliites();
        }

        //UpdateDash();
    }

    //private void UpdateDash()
    //{
    //    if (dashCooldownTimer != null && usedDash == true)
    //        dashCooldownTimer.UpdateClock();

    //    if (dashTimer != null && playerController.isDashActive == true)
    //        dashTimer.UpdateClock();
    //}

    //private void BeginDash()
    //{
    //    if (usedDash == true)
    //        return;

    //    usedDash = true;
    //    playerController.isDashActive = true;
    //    owner.AnimHelper.PlayOrStopAnimBool("Dashing", true);

    //    StatAdjustmentManager.ApplyTrackedStatMod(owner.EntityStats, owner.EntityStats, BaseStat.StatType.MoveSpeed, mod);
    //}

    //private void EndDash()
    //{

    //    playerController.isDashActive = false;
    //    owner.AnimHelper.PlayOrStopAnimBool("Dashing", false);

    //    StatAdjustmentManager.RemoveTrackedStatMod(owner.EntityStats, owner.EntityStats, BaseStat.StatType.MoveSpeed, mod);
    //}

    //private void RefreshDash()
    //{
    //    //Debug.Log("Refreshing Dash");
    //    usedDash = false;
    //}
}
