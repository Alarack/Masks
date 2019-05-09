using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AddForceHelper 
{


    public static void AddRandomForce(Rigidbody2D rb, float minXForce, float maxXForce, float minYForce, float maxYForce, float minRot, float maxRot, float gravity = 1.5f)
    {
        //Rigidbody2D rb = part.gameObject.AddComponent<Rigidbody2D>();
        float xForce = Random.Range(minXForce, maxXForce);
        float yForce = Random.Range(minYForce, maxYForce);
        float rotSpeed = Random.Range(minRot, maxRot);

        rb.angularVelocity = rotSpeed;
        rb.velocity = new Vector2(xForce, yForce);
    }

}
