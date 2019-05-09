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

    [Header("Dash Variables")]
    public float dashDuration = 0.5f;
    //public float dashSpeed = 30f;
    public float dashCooldown = 3f;

    [Header("Attack Variable")]
    public GameObject attackGameObject;
    public GameObject attackGameObjectVariant;
    public Transform leftOrigin;
    public Transform rightOrigin;

    private GameObject currentAttackGameObject;


    //public bool isDashActive;


    public System.Action onCollideWithGround;

    public override void Initialize(Entity owner)
    {
        base.Initialize(owner);
    }

    private void Start()
    {
        //FSM TESTING
        FSMState normalState = Owner.FSMManager.GetState("PlayerNormal");
        if (normalState != null)
            Owner.EntityFSM.ChangeState(normalState);
        else
        {
            Debug.LogError("Can't find normal state");
        }
    }



    protected override void Update()
    {
        base.Update();
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


        if (RayController.IsHittingWall && RayController.IsGrounded == false && MyBody.velocity.y <= 0)
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

    //public override void MoveHorizontal()
    //{
    //    base.MoveHorizontal();
    //}

    #region ATTACKS

    //private void Attack()
    //{
    //    currentAttackGameObject = attackGameObject;
    //    Owner.AnimHelper.SetAnimEventAction(CreateAttackInstance);
    //    Owner.AnimHelper.PlayAnimTrigger("Attack1");
    //}

    //private void Attack2()
    //{
    //    currentAttackGameObject = attackGameObjectVariant;
    //    Owner.AnimHelper.SetAnimEventAction(CreateAttackInstance);
    //    Owner.AnimHelper.PlayAnimTrigger("Attack1");
    //}

    //private void CreateAttackInstance()
    //{
    //    Transform origin = GetAttackOriginByFacing();

    //    GameObject attack = Instantiate(currentAttackGameObject, origin.transform.position, currentAttackGameObject.transform.rotation) as GameObject;
    //    attack.transform.SetParent(origin, false);
    //    attack.transform.localPosition = Vector2.zero;
    //    HitBox hit = attack.GetComponent<HitBox>();

    //    Vector2 force = new Vector2(hit.xForce, hit.yForce);

    //    hit.SetKnockBack(CalcKnockBack(force));
    //    if (GetFacing() == FacingDirection.Left)
    //    {
    //        attack.transform.localScale = new Vector3(attack.transform.localScale.x * -1, 1, 1);
    //    }
    //    //Debug.Log(attack.name + " Created");
    //}

    //private Vector2 CalcKnockBack(Vector2 knockBack)
    //{
    //    Vector2 result = knockBack;

    //    if (Facing == FacingDirection.Left)
    //    {
    //        result = new Vector2(knockBack.x * -1, knockBack.y);
    //    }

    //    return result;
    //}

    //private Transform GetAttackOriginByFacing()
    //{
    //    FacingDirection currentFacing = GetFacing();
    //    return currentFacing == FacingDirection.Left ? leftOrigin : rightOrigin;
    //}

    #endregion

    protected override void UpdateFacing()
    {
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
    }


    private void VariableFall()
    {
        Vector2 desiredFallVelocity = Vector2.zero;

        if (MyBody.velocity.y < 0)
        {
            desiredFallVelocity = Vector2.up * Physics2D.gravity.y * descendingFallMod * Time.deltaTime;
        }
        else if (MyBody.velocity.y > 0 && GameInput.JumpHeld == false)
        {
            desiredFallVelocity = Vector2.up * Physics2D.gravity.y * ascendingFallMod * Time.deltaTime;
        }

        MyBody.velocity += desiredFallVelocity;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (LayerTools.IsLayerInMask(groundLayer, other.gameObject.layer) == false)
            return;

        //Debug.Log("Collided with ground");

        if (onCollideWithGround != null)
            onCollideWithGround();

    }








}