using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public enum DeathType {
        Explode,
        PopHead
    }

    public bool randomDeathType;
    public DeathType deathType;
    public GameObject bloodFountain;
    public Transform bloodLocation;
    private CorpsePart[] bodyParts;


    public void Initialize()
    {
        bodyParts = GetComponentsInChildren<CorpsePart>();

        int count = bodyParts.Length;
        for (int i = 0; i < count; i++)
        {
            bodyParts[i].Initialize();
        }


        if (randomDeathType)
        {
            deathType = (DeathType)Random.Range(0, System.Enum.GetValues(typeof(DeathType)).Length);
        }

        Destroy(gameObject, 5f);
    }


    public void PlayDeathEffect()
    {
        switch (deathType)
        {
            case DeathType.Explode:
                Explode();
                break;

            case DeathType.PopHead:
                PopHead();
                break;
        }
    }


    public void PopHead()
    {
        CorpsePart head = GetPart(CorpsePart.PartType.Head);
        if (head == null)
            return;

        AddRigidBodyAndForce(head, -2f, 2f, 5f, 15f, -360f, 360f);

        GameObject blood = Instantiate(bloodFountain, bloodLocation.position, bloodLocation.transform.rotation) as GameObject;
        blood.transform.SetParent(bloodLocation, true);
        //Destroy(blood, 5f);
    }


    public void Explode()
    {
        int count = bodyParts.Length;
        for (int i = 0; i < count; i++)
        {
            CorpsePart currentPart = bodyParts[i];

            AddRigidBodyAndForce(currentPart, -5f, 5f, 5f, 15f, -1080f, 1080f, 1.5f);


            //Rigidbody2D rb = currentPart.gameObject.AddComponent<Rigidbody2D>();
            //rb.gravityScale = 1.5f;

            //float randomRotation = Random.Range(-1080f, 1080f);
            //rb.angularVelocity = randomRotation;

            //float xForce = Random.Range(-5f, 5f);
            //float yForce = Random.Range(5f, 15f);

            //rb.velocity = new Vector2(xForce, yForce);
        }
    }


    public CorpsePart GetPart(CorpsePart.PartType type)
    {
        int count = bodyParts.Length;
        for (int i = 0; i < count; i++)
        {
            if (bodyParts[i].partType == type)
                return bodyParts[i];
        }


        return null;
    }



    private void AddRigidBodyAndForce(CorpsePart part, float minXForce, float maxXForce, float minYForce, float maxYForce, float minRot, float maxRot, float gravity = 1.5f)
    {
        Rigidbody2D rb = part.gameObject.AddComponent<Rigidbody2D>();
        float xForce = Random.Range(minXForce, maxXForce);
        float yForce = Random.Range(minYForce, maxYForce);
        float rotSpeed = Random.Range(minRot, maxRot);

        rb.angularVelocity = rotSpeed;
        rb.velocity = new Vector2(xForce, yForce);
    }



}
