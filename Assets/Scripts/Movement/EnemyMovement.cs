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

        switch (GameManager.Instance.dimensionMode)
        {
            case DimensionMode.Two:
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
                break;

            case DimensionMode.Three:
                if (GameInput.Horizontal < 0 && currentYRotation != leftYRotation)
                {
                    desiredYRotation = leftYRotation;
                    SwapWeaponSide();
                }

                if (GameInput.Horizontal > 0 && currentYRotation != rightYRotation)
                {
                    desiredYRotation = rightYRotation;
                    SwapWeaponSide();
                }


                if (currentYRotation != desiredYRotation)
                {
                    currentYRotation = Mathf.MoveTowardsAngle(currentYRotation, desiredYRotation, 15f * Time.deltaTime);

                    model.transform.localEulerAngles = new Vector3(0f, currentYRotation, 0f);
                }
                break;
        }


    }

   

    public void FlipDirection()
    {
        if (recentFlip == true)
            return;

        recentFlip = true;
        switch (GameManager.Instance.dimensionMode)
        {
            case DimensionMode.Two:
                Owner.SpriteRenderer.flipX = !Owner.SpriteRenderer.flipX;
                break;

            case DimensionMode.Three:
                desiredYRotation = Facing == FacingDirection.Left ? rightYRotation : leftYRotation;
                break;
        }
        
    }

    private void ResetFlipTimer()
    {
        recentFlip = false;
    }


}
