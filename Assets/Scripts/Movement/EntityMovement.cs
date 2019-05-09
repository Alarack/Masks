using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour {


    public enum FacingDirection {
        Left,
        Right
    }

    public FacingDirection Facing { get { return GetFacing(); } }
    public Entity Owner { get; protected set; }
    public Rigidbody2D MyBody { get; private set; }
    public BoxCollider2D BoxCollider { get; private set; }

    public float Speed { get { return Owner.EntityStats.GetStatModifiedValue(BaseStat.StatType.MoveSpeed); } }

    protected float currentHorizontalDirection;
    //public float desiredSpeed;
    //public float baseMoveSpeed;

    [Header("Layer Masks")]
    public LayerMask groundLayer;

    public RayCastController RayController { get; protected set; }


    //[Header("Flags for temp status")]
    //public bool knockedBack;

    private Timer knockBackTimer;



    protected virtual void Update()
    {
        if (RayController != null)
            RayController.ManagedUpdate();

        ConfigureHorizontalDirection();

        //if (knockBackTimer != null && knockedBack == true)
        //{
        //    knockBackTimer.UpdateClock();
        //}
    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void MoveHorizontal()
    {

        //Debug.Log(Speed + " is speed for " + Owner.gameObject.name);

        if(Speed == 0)
        {
            Owner.AnimHelper.StopWalk();
        }


        bool underMovementAffecting = StatusManager.CheckForStatus(Owner.gameObject, Constants.StatusType.MovementAffecting);
        bool underKnockback = StatusManager.CheckForStatus(Owner.gameObject, Constants.StatusType.Knockback);

        if (underMovementAffecting == true || underKnockback == true)
        {
            //if(Owner is EntityEnemy)
            //{
            //    Debug.Log(" I havea movement affecting status");
            //}

            return;
        }


        if (currentHorizontalDirection == 0f)
        {
            Owner.AnimHelper.StopWalk();

            //if (knockedBack == true)
            //    return;
        }
        else
        {
            if (RayController.IsGrounded)
                Owner.AnimHelper.PlayWalk();
        }

        //if (Owner is EntityEnemy)
        //    Debug.Log("No movement affecting status, so moving normal");

        MyBody.velocity = new Vector2(currentHorizontalDirection * Speed, MyBody.velocity.y);
    }

    protected virtual void ConfigureHorizontalDirection()
    {

    }

    public virtual void Initialize(Entity owner)
    {
        Owner = owner;
        MyBody = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<BoxCollider2D>();

        RayController = new RayCastController(this);
    }

    public FacingDirection GetFacing()
    {
        return Owner.SpriteRenderer.flipX ? FacingDirection.Left : FacingDirection.Right;
    }

    protected virtual void UpdateFacing()
    {

    }

    protected void SwapWeaponSide()
    {
        //Debug.Log("Swaping weapon side");

        if (Owner.CurrentWeapon == null)
        {
            //Debug.Log("weapon is null");
            return;
        }


        EffectOriginPoint originPoint = Owner.EffectDelivery.GetCurrentWeaponPoint() ?? new EffectOriginPoint();


        //Debug.Log(originPoint.originType + " is the origion point type");

        if (originPoint.point != null)
        {
            switch (originPoint.originType)
            {
                case Constants.EffectOrigin.RightHand:

                    if(Owner.Movement.Facing == FacingDirection.Left)
                    {
                        Owner.CurrentWeapon.transform.SetParent(Owner.EffectDelivery.GetOriginPoint(Constants.EffectOrigin.LeftHand), false);
                        Owner.CurrentWeapon.transform.localScale = new Vector3(
                            Owner.CurrentWeapon.transform.localScale.x * -1,
                            Owner.CurrentWeapon.transform.localScale.y,
                            Owner.CurrentWeapon.transform.localScale.z);
                    }

                    break;

                case Constants.EffectOrigin.LeftHand:
                    if(Owner.Movement.Facing == FacingDirection.Right)
                    {
                        Owner.CurrentWeapon.transform.SetParent(Owner.EffectDelivery.GetOriginPoint(Constants.EffectOrigin.RightHand), false);
                        Owner.CurrentWeapon.transform.localScale = new Vector3(
                            Owner.CurrentWeapon.transform.localScale.x * -1,
                            Owner.CurrentWeapon.transform.localScale.y,
                            Owner.CurrentWeapon.transform.localScale.z);
                    }

                    break;


                case Constants.EffectOrigin.CharacterFront:
                    Transform target = null;

                    Transform left = Owner.EffectDelivery.GetOriginPoint(Constants.EffectOrigin.LeftHand);
                    Transform right = Owner.EffectDelivery.GetOriginPoint(Constants.EffectOrigin.RightHand);

                    target = Owner.Movement.Facing == EntityMovement.FacingDirection.Left ? left : right;


                    Debug.Log(target.gameObject.name + " is where I should be");
                    Debug.Log(originPoint.point + " is where I am");

                    if (originPoint.point != target)
                    {
                        Debug.Log("wrong side");
                    }

                    break;
            }
        }

    }


    public virtual void SetFacing(FacingDirection direction)
    {
        FacingDirection currentFacing = GetFacing();

        if (currentFacing == direction)
            return;

        switch (direction)
        {
            case FacingDirection.Left:
                Owner.SpriteRenderer.flipX = true;
                break;
            case FacingDirection.Right:
                Owner.SpriteRenderer.flipX = false;
                break;

        }

        //Debug.Log("facing: " + Facing);
        //Debug.Log("dir: " + currentHorizontalDirection);
    }


    //private void RestoreKnockBack()
    //{
    //    knockedBack = false;
    //    knockBackTimer = null;
    //}


    public void ForceMovement(Vector2 force, float duration = 0.2f, bool resetVelocity = false)
    {
        //knockedBack = true;
        //if(knockBackTimer == null)
        //    knockBackTimer = new Timer(duration, RestoreKnockBack);
        //else
        //{
        //    knockBackTimer.ModifyDuration(duration);
        //}

        if (resetVelocity == true)
            MyBody.velocity = Vector2.zero;

        MyBody.velocity += force;
        
    }

    public void SpinCrazy()
    {
        float randongRotSpeed = Random.Range(-720f, 720f);
        float randomY = Random.Range(250f, 350f);

        Vector2 force = new Vector2(MyBody.velocity.x, randomY);
        MyBody.freezeRotation = false;
        MyBody.gravityScale = 1.5f;
        MyBody.angularVelocity = randongRotSpeed;
    }


}
