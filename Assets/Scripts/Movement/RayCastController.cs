using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FacingDirection = EntityMovement.FacingDirection;

public class RayCastController 
{
    protected const float SKIN_WIDTH = 0.015f;
    protected const float DISTANCE_BETWEEN_RAYS = 0.25f;

    
    public bool IsHittingWall { get; protected set; }
    public bool IsGrounded { get; protected set; }
    public bool IsAtLedge { get; protected set; }

    protected RaycastOrigins rayOrigins;
    protected int horizontalRayCount;
    protected int verticalRayCount;
    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    protected BoxCollider2D boxCollider;
    protected EntityMovement movement;

    public RayCastController(EntityMovement movement)
    {
        this.movement = movement;
        //rayOrigins = new RaycastOrigins();

        boxCollider = movement.BoxCollider;
        CalculateRaySpacing();
    }

    public void ManagedUpdate()
    {
        UpdateRaycastOrigins();
        DetectWall();
        CheckGround();
        CheckForLedge();
    }



    protected void UpdateRaycastOrigins()
    {
        Bounds bounds = GetBounds();

        rayOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    protected void CalculateRaySpacing()
    {
        Bounds bounds = GetBounds();

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / DISTANCE_BETWEEN_RAYS);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / DISTANCE_BETWEEN_RAYS);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }


    protected Bounds GetBounds()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(SKIN_WIDTH * -2);

        return bounds;
    }




    protected void DetectWall()
    {
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = movement.Facing == FacingDirection.Left ? rayOrigins.bottomLeft : rayOrigins.bottomRight;
            Vector2 direction = movement.Facing == FacingDirection.Left ? Vector2.left : Vector2.right;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 0.25f, movement.groundLayer);

            Debug.DrawRay(rayOrigin, direction, Color.red);




            if (hit.collider != null && /*IsGrounded == false &&*/ movement.MyBody.velocity.y <= 0f)
            {
                IsHittingWall = true;
                //Debug.Log("Hitting wall");
                break;
            }
            else
            {
                IsHittingWall = false;
            }
        }
    }

    private void CheckGround()
    {
        float rayLength = 0.5f;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = rayOrigins.bottomLeft;
            rayOrigin += (Vector2.right * verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, movement.groundLayer);

            if (hit.collider != null)
            {
                IsGrounded = true;
                //Debug.Log("I'm grounded");
                break;
            }
            else
            {
                IsGrounded = false;
            }
        }
    }

    private void CheckForLedge()
    {
        float rayLength = 0.25f;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = rayOrigins.bottomLeft;
            rayOrigin += (Vector2.right * verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, movement.groundLayer);

            if (hit.collider == null && IsGrounded == true)
            {
                IsAtLedge = true;
                break;
            }
            else
            {
                IsAtLedge = false;
            }
        }
    }



    public struct RaycastOrigins {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }


}
