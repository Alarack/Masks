using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceForce;
    [Range(0f, 1f)]
    public float diminishRate = 0.5f;
    public LayerMask bounceLayer;

    private Rigidbody2D myBody;
    private float startingForce;


    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        startingForce = bounceForce;
    }

    private void ActivateBounce()
    {
        myBody.AddForce(Vector2.up * bounceForce);

        bounceForce *= diminishRate;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (LayerTools.IsLayerInMask(bounceLayer, other.gameObject.layer) == false)
            return;

        ActivateBounce();

        //if (myBody.velocity.magnitude >= 5f)
        //    bounceForce = startingForce;
    }



}
