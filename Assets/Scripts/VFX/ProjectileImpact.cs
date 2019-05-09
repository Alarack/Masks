using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileImpact : MonoBehaviour
{
    [Header("Forces")]
    public float minXForce = -5f;
    public float maxXForce = 5f;
    public float minYForce = 5f;
    public float maxYForce = 15f;
    public float minRot = -1080f;
    public float maxRot = 1080f;
    public float gravity = 1.5f;

    [Header("Life")]
    public float life;


    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        AddForceHelper.AddRandomForce(rb, minXForce, maxXForce, minYForce, maxYForce, minRot, maxRot);
        Destroy(gameObject, life);
    }



}
