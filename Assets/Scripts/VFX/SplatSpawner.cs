using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatSpawner : MonoBehaviour
{
    public ParticleSystem splatParticles;
    //public GameObject splatPrefab;
    public ObjectPoolManager.ObjectPoolType poolType;
    //public Transform splatHolder;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {

        ParticlePhysicsExtensions.GetCollisionEvents(splatParticles, other, collisionEvents);

        int count = collisionEvents.Count;


        //Debug.Log("---------------- PARTICLE COLLISIONS " + count);

        for (int i = 0; i < count; i++)
        {

            float create = Random.Range(0f, 100f);

            if (create < 50f)
            {
                //Debug.Log("Skip splat -----------------------------------------");
                return;
            }

            List<PooledObject> activeObjects = GameManager.Instance.objectPools.GetActiveObjectsWithinArea(collisionEvents[i].intersection, 0.5f);

            //Debug.Log(activeObjects.Count + " found nearby");

            if(activeObjects.Count > 75)
            {
                return;
            }

            ObjectPoolManager.PoolRequestInfo info = ObjectPoolManager.CreatePoolInfo(
                poolType,
                GameManager.Instance.splatHolder,
                collisionEvents[i].intersection,
                null,
                true,
                false
                );


            PooledObject pooledSplat = GameManager.GetPooledObject(info);

            if (pooledSplat == null)
            {
                //Debug.LogError("Out of Splats");
                return;
            }

            Splat splat = pooledSplat as Splat;
            splat.SetSplatVisualLayer(Splat.SplatLoacation.Foreground);




            //GameObject splat = Instantiate(splatPrefab, collisionEvents[i].intersection, Quaternion.identity) as GameObject;
            //splat.transform.SetParent(GameManager.Instance.splatHolder, true);
            //Splat splatScript = splat.GetComponent<Splat>();
            //splatScript.Initialize(Splat.SplatLoacation.Foreground);
            

            //Debug.Log("Creations : " + GameManager.splatCreations + " ---------------------------------------");
        }
    }
}
