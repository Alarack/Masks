using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{

    public float zombieWanderInterval;
    [Range(0f, 1f)]
    public float zombieFlipChance;


    private bool recentFlip;
    private Timer flipCooldown;




    public override void Initialize(Entity owner)
    {
        base.Initialize(owner);
        flipCooldown = new Timer(0.25f, ResetFlipTimer, true);
    }

    protected override void Update()
    {
        base.Update();

        if (recentFlip == true && flipCooldown != null)
        {
            flipCooldown.UpdateClock();
        }
    }

    //public override void MoveHorizontal()
    //{
    //    base.MoveHorizontal();
    //}

    protected override void ConfigureHorizontalDirection()
    {

        if (StatusManager.CheckForStatus(Owner.gameObject, Constants.StatusType.MovementAffecting) == true)
        {
            currentHorizontalDirection = 0f;
        }
        else
        {
            currentHorizontalDirection = Facing == FacingDirection.Left ? -1 : 1;
        }

        //Debug.Log("facing: " + Facing);
        //Debug.Log("dir: " + currentHorizontalDirection);


        //desiredSpeed = baseMoveSpeed;
        UpdateFacing();
    }

    protected override void UpdateFacing()
    {
        if (Owner == null)
            return;


        if (currentHorizontalDirection < 0 && Owner.SpriteRenderer.flipX == false)
        {
            Owner.SpriteRenderer.flipX = true;
            SwapWeaponSide();
        }

        if (currentHorizontalDirection > 0 && Owner.SpriteRenderer.flipX == true)
        {
            Owner.SpriteRenderer.flipX = false;
            SwapWeaponSide();
        }
    }

   

    public void FlipDirection()
    {
        if (recentFlip == true)
            return;

        recentFlip = true;
        Owner.SpriteRenderer.flipX = !Owner.SpriteRenderer.flipX;
    }

    private void ResetFlipTimer()
    {
        recentFlip = false;
    }


}
