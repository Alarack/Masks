using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAction : BaseStateAction
{
    private Timer wanderTimer;
    private EnemyMovement enemyMovement;


    public WanderAction(Entity owner, bool runUpdate):base (owner, runUpdate)
    {
        //Debug.Log("creating a wander action for " + owner.gameObject.GetInstanceID());

        enemyMovement = owner.Movement as EnemyMovement;
        wanderTimer = new Timer(enemyMovement.zombieWanderInterval, Wander, true);
    }

    public override void Execute()
    {
        enemyMovement.MoveHorizontal();
    }

    public override void ManagedUpdate()
    {
        if (wanderTimer != null)
            wanderTimer.UpdateClock();
    }

    private void Wander()
    {
        float random = Random.Range(0f, 1f);
        if (random < enemyMovement.zombieFlipChance)
        {
            enemyMovement.FlipDirection();
            //Debug.Log("Wander flip");
        }
    }



}
