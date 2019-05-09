using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJumpAction : BaseStateAction {

    private PlayerController playerController;
    private int currentJumpCount;
    private Action jumpResetCallBack;


    public PlayerJumpAction(Entity owner, bool runUpdate) : base(owner, runUpdate)
    {
        currentJumpCount = 0;
        playerController = owner.Movement as PlayerController;
    }

    public override void RegisterEvents()
    {
        playerController.onCollideWithGround += ResetJump;
    }

    public override void UnregisterEvents()
    {
        playerController.onCollideWithGround -= ResetJump;
    }

    public override void Execute()
    {
        if (GameInput.Jump)
        {
            Jump();
        }

        //ResetJump();

        //Debug.Log(currentJumpCount + " is current jumps");
    }


    private void Jump()
    {
        //Debug.Log("Trying to Jump");

        float desiredJumpForce = playerController.RayController.IsGrounded ? playerController.jumpForce : playerController.arialJumpForce;

        if (currentJumpCount >= playerController.maxJumpCount)
        {
            //Debug.Log("TOo many jumps: " + currentJumpCount);
            return;
        }

        owner.AnimHelper.PlayOrStopAnimBool("Jumping", true);

        playerController.MyBody.velocity = new Vector2(playerController.MyBody.velocity.x, 0f);

        playerController.MyBody.AddForce(Vector2.up * desiredJumpForce);

        currentJumpCount++;
    }

    private void ResetJump()
    {
        if (/*playerController.MyBody.velocity.y <= 0f &&*/ currentJumpCount > 0 /*&& playerController.RayController.IsGrounded == true*/)
        {
            //Debug.Log("Reseting jump");

            currentJumpCount = 0;
            owner.AnimHelper.PlayOrStopAnimBool("Jumping", false);
        }
    }
}
