using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAction : BaseStateAction {


    PlayerController playerController;

    public PlayerMoveAction(Entity owner, bool seperateUpdate) : base(owner, seperateUpdate)
    {
        playerController = owner.Movement as PlayerController;
    }

    public override void Execute()
    {
        playerController.MoveHorizontal();
    }



}
