using UnityEngine;

public class EntityMovement : MonoBehaviour
{


    public const float leftYRotation = 270f;
    public const float rightYRotation = 90f;

    public enum FacingDirection
    {
        Left,
        Right
    }

    public FacingDirection Facing { get { return GetFacing(); } }
    public float currentYRotation;
    public float desiredYRotation;
    public GameObject model;
    public Entity Owner { get; protected set; }

    ////2D
    //public Rigidbody2D My2DBody { get { return MyPhysics.My2DBody; } }
    //public BoxCollider2D Box2DCollider { get { return MyPhysics.Box2DCollider; } }

    ////3D
    //public Rigidbody My3DBody { get { return MyPhysics.My3DBody; } }
    //public CapsuleCollider MyCapsuelCollider { get { return MyPhysics.MyCapsuelCollider; } }

    public PhysicsInfo MyPhysics { get; protected set; }

    public float Speed { get { return Owner.EntityStats.GetStatModifiedValue(BaseStat.StatType.MoveSpeed); } }

    protected float currentHorizontalDirection;
    //public float desiredSpeed;
    //public float baseMoveSpeed;

    [Header("Layer Masks")]
    public LayerMask groundLayer;

    protected RayCastController ray2DController;
    protected RayCast3DController ray3DController;

    public bool IsGrounded { get { return GetGrounded(); } }
    public bool IsAtLedge { get { return GetAtLedge(); } }
    public bool IsHittingWall { get { return GetHittingWall(); } }

    //[Header("Flags for temp status")]
    //public bool knockedBack;

    private Timer knockBackTimer;

    public virtual void Initialize(Entity owner)
    {
        MyPhysics = new PhysicsInfo(gameObject,
            GetComponent<Rigidbody2D>(),
            GetComponent<Rigidbody>(),
            GetComponent<BoxCollider2D>(),
            GetComponent<CapsuleCollider>());

        //Debug.Log("Initializig entity movemment for " + owner.gameObject.name);
        Owner = owner;

        ray3DController = new RayCast3DController(this);

    }


    protected virtual void Update()
    {

        switch (GameManager.Instance.dimensionMode)
        {
            case DimensionMode.Three:
                ray3DController.ManagedUpdate();
                break;

            case DimensionMode.Two:
                //ray2DController.ManagedUpdate();
                break;
        }

        ConfigureHorizontalDirection();
    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void MoveHorizontal()
    {
        //Debug.Log(Speed + " is speed for " + Owner.gameObject.name);
        if (Speed == 0)
        {
            Owner.AnimHelper.StopWalk();
        }

        bool underMovementAffecting = StatusManager.CheckForStatus(Owner.gameObject, Constants.StatusType.MovementAffecting);
        bool underKnockback = StatusManager.CheckForStatus(Owner.gameObject, Constants.StatusType.Knockback);

        if (underMovementAffecting == true || underKnockback == true)
        {
            return;
        }

        if (currentHorizontalDirection == 0f)
        {
            Owner.AnimHelper.StopWalk();
            //Debug.Log("Not walking");
        }
        else
        {
            if (IsGrounded)
            {
                //Debug.Log("Walking");
                Owner.AnimHelper.PlayWalk();
            }
        }

        MyPhysics.SetVelocity(new Vector3(currentHorizontalDirection * Speed, MyPhysics.Velocity.y, 0f));
    }

    protected virtual void ConfigureHorizontalDirection()
    {

    }



    public FacingDirection GetFacing()
    {
        switch (GameManager.Instance.dimensionMode)
        {
            case DimensionMode.Two:
                return Owner.SpriteRenderer.flipX ? FacingDirection.Left : FacingDirection.Right;

            case DimensionMode.Three:
                return currentYRotation == leftYRotation ? FacingDirection.Left : FacingDirection.Right;

            default:
                return Owner.SpriteRenderer.flipX ? FacingDirection.Left : FacingDirection.Right;
        }




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

                    if (Owner.Movement.Facing == FacingDirection.Left)
                    {
                        Owner.CurrentWeapon.transform.SetParent(Owner.EffectDelivery.GetOriginPoint(Constants.EffectOrigin.LeftHand), false);
                        Owner.CurrentWeapon.transform.localScale = new Vector3(
                            Owner.CurrentWeapon.transform.localScale.x * -1,
                            Owner.CurrentWeapon.transform.localScale.y,
                            Owner.CurrentWeapon.transform.localScale.z);
                    }

                    break;

                case Constants.EffectOrigin.LeftHand:
                    if (Owner.Movement.Facing == FacingDirection.Right)
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

        switch (GameManager.Instance.dimensionMode)
        {
            case DimensionMode.Two:
                Owner.SpriteRenderer.flipX = direction == FacingDirection.Left ? true : false;
                break;

            case DimensionMode.Three:
                desiredYRotation = direction == FacingDirection.Left ? leftYRotation : rightYRotation;
                break;
        }
    }


    public bool GetGrounded()
    {
        if (GameManager.Instance.dimensionMode == DimensionMode.Two)
            return ray2DController.IsGrounded;
        else
            return ray3DController.IsGrounded;
    }

    public bool GetHittingWall()
    {
        if (GameManager.Instance.dimensionMode == DimensionMode.Two)
            return ray2DController.IsHittingWall;
        else
            return ray3DController.IsHittingWall;
    }

    public bool GetAtLedge()
    {
        if (GameManager.Instance.dimensionMode == DimensionMode.Two)
            return ray2DController.IsAtLedge;
        else
            return ray3DController.IsAtLedge;
    }



}


public class PhysicsInfo
{
    //2D
    public Rigidbody2D My2DBody { get; private set; }
    public BoxCollider2D Box2DCollider { get; private set; }

    //3D
    public Rigidbody My3DBody { get; private set; }
    public CapsuleCollider MyCapsuelCollider { get; private set; }

    public GameObject Owner { get; private set; }

    public Vector3 Velocity { get { return GetVelocity(); } }


    public PhysicsInfo(GameObject owner, Rigidbody2D rb2d, Rigidbody rb, Collider2D col2d, Collider col3d)
    {
        if (col2d is BoxCollider2D)
            this.Box2DCollider = col2d as BoxCollider2D;

        if (col3d is CapsuleCollider)
            this.MyCapsuelCollider = col3d as CapsuleCollider;

        this.Owner = owner;
        this.My2DBody = rb2d;
        this.My3DBody = rb;

    }

    public void AddForce(Vector3 force)
    {
        if (GameManager.Instance.dimensionMode == DimensionMode.Two)
            My2DBody.AddForce(force);
        else
            My3DBody.AddForce(force);
    }
    public void AddVelocity(Vector3 velocity)
    {
        if (GameManager.Instance.dimensionMode == DimensionMode.Two)
            My2DBody.velocity += (Vector2)velocity;
        else
            My3DBody.velocity += velocity;
    }

    public void ResetVelocity()
    {
        if (GameManager.Instance.dimensionMode == DimensionMode.Two)
            My2DBody.velocity = Vector2.zero;
        else
            My3DBody.velocity = Vector3.zero;
    }

    public void SetVelocity(Vector3 velocity)
    {
        if (GameManager.Instance.dimensionMode == DimensionMode.Two)
            My2DBody.velocity = velocity;
        else
            My3DBody.velocity = velocity;
    }

    public Vector3 GetVelocity()
    {
        if (GameManager.Instance.dimensionMode == DimensionMode.Two)
            return My2DBody.velocity;
        else
            return My3DBody.velocity;
    }

}
