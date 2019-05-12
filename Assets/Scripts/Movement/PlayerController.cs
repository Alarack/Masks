using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LL.FSM;

public class PlayerController : EntityMovement {

    public enum JumpType {
        Standard,
        Variable
    }

    public enum MoveState {
        Standing,
        Running,
        Dashing
    }


    [Header("Temp Jump Variables")]
    public JumpType jumpType = JumpType.Standard;
    public float jumpForce = 1200f;
    public float arialJumpForce = 500f;

    public int maxJumpCount = 1;

    [Header("Variable Jump Variables")]
    public float descendingFallMod = 1.5f;
    public float ascendingFallMod = 1f;


    public System.Action onCollideWithGround;

    public override void Initialize(Entity owner)
    {
        base.Initialize(owner);

       // Debug.Log("Player controller has been initialized");

        SwapToDefaultState();
    }


    private void SwapToDefaultState()
    {
        Owner.InitFSM();


        //FSM TESTING
        FSMState normalState = Owner.FSMManager.GetState("PlayerNormal");
        if (normalState != null)
            Owner.EntityFSM.ChangeState(normalState);
        else
        {
            Debug.LogError("Can't find normal state");
        }
    }

    protected override void ConfigureHorizontalDirection()
    {

        if (StatusManager.CheckForStatus(Owner.gameObject, Constants.StatusType.MovementAffecting) == true)
        {

            //currentHorizontalDirection = Facing == FacingDirection.Left ? -1 : 1;
            return;

        }
        else if (StatusManager.CheckForStatus(Owner.gameObject, Constants.StatusType.ForceMaxSpeed) == true)
        {
            currentHorizontalDirection = Facing == FacingDirection.Left ? -1 : 1;
        }
        else
        {
            currentHorizontalDirection = GameInput.Horizontal;
        }


        if (IsHittingWall && IsGrounded == false && MyPhysics.Velocity.y <= 0)
        {
            currentHorizontalDirection = 0f;
        }
        UpdateFacing();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (jumpType == JumpType.Variable)
            VariableFall();
    }

    protected override void UpdateFacing()
    {

        switch (GameManager.Instance.dimensionMode)
        {
            case DimensionMode.Two:
                if (GameInput.Horizontal < 0 && Owner.SpriteRenderer.flipX == false)
                {
                    Owner.SpriteRenderer.flipX = true;
                    SwapWeaponSide();
                }

                if (GameInput.Horizontal > 0 && Owner.SpriteRenderer.flipX == true)
                {
                    Owner.SpriteRenderer.flipX = false;
                    SwapWeaponSide();
                }
                break;

            case DimensionMode.Three:
                if(GameInput.Horizontal < 0 && currentYRotation != leftYRotation)
                {
                    desiredYRotation = leftYRotation;
                    SwapWeaponSide();
                }

                if(GameInput.Horizontal > 0 && currentYRotation != rightYRotation)
                {
                    desiredYRotation = rightYRotation;
                    SwapWeaponSide();
                }
                

                if(currentYRotation != desiredYRotation)
                {
                    currentYRotation = Mathf.MoveTowardsAngle(currentYRotation, desiredYRotation, Owner.EntityStats.GetStatModifiedValue(BaseStat.StatType.RotateSpeed) * Time.deltaTime);

                    model.transform.localEulerAngles = new Vector3(0f, currentYRotation, 0f);
                }


                break;
        }



    }


    private void VariableFall()
    {
        Vector2 desiredFallVelocity = Vector2.zero;

        if (MyPhysics.Velocity.y < 0)
        {
            desiredFallVelocity = Vector2.up * Physics2D.gravity.y * descendingFallMod * Time.deltaTime;
        }
        else if (MyPhysics.Velocity.y > 0 && GameInput.JumpHeld == false)
        {
            desiredFallVelocity = Vector2.up * Physics2D.gravity.y * ascendingFallMod * Time.deltaTime;
        }


        MyPhysics.AddVelocity(desiredFallVelocity);
        //My2DBody.velocity += desiredFallVelocity;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (LayerTools.IsLayerInMask(groundLayer, other.gameObject.layer) == false)
            return;

        //Debug.Log("Collided with ground");

        if (onCollideWithGround != null)
            onCollideWithGround();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (LayerTools.IsLayerInMask(groundLayer, other.gameObject.layer) == false)
            return;

        //Debug.Log("Collided with ground");

        if (onCollideWithGround != null)
            onCollideWithGround();
    }








}