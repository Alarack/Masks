using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PoolType = ObjectPool<Splat>.PoolType;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool {
        [Header("Pool Info")]
        public ObjectPoolType type;
        public PooledObject prefab;
        //public List<ObjectPoolVarient> variants = new List<ObjectPoolVarient>();
        public Transform holder;
        public int size;
    }

    //[System.Serializable]
    //public class ObjectPoolVarient {
    //    public PoolVariantType variant;
    //    public PooledObject prefab;
    //}

    public enum ObjectPoolType { // Pools Need Variants for colors. Make a calls for pool type instead of just this enum
        Splat,
        Limb,
        Corpse,
        GreenSplat
    }
    //public enum PoolVariantType {
    //    None,
    //    Red,
    //    Green
    //}

    [Header("Pools")]
    public List<Pool> pools = new List<Pool>();

    private Dictionary<ObjectPoolType, Queue<PooledObject>> poolDictionary = new Dictionary<ObjectPoolType, Queue<PooledObject>>();
    private List<PooledObject> activeObjects = new List<PooledObject>();


    private void Start()
    {
        PopulatePools();
    }

    private void PopulatePools()
    {
        int count = pools.Count;
        for (int i = 0; i < count; i++)
        {
            Queue<PooledObject> newPool = new Queue<PooledObject>();

            poolDictionary.Add(pools[i].type, newPool);

            for (int j = 0; j < pools[i].size; j++)
            {
                CreateAndAddObject(pools[i].type);
            }
        }
    }

    private void CreateAndAddObject(ObjectPoolType poolType)
    {
        Queue<PooledObject> targetPool = GetQueue(poolType);

        if(targetPool == null)
        {
            Debug.LogError("Pool null");
            return;
        }

        PooledObject targetObject = GetPrefabFromPool(poolType);

        PooledObject newObject = Instantiate(targetObject) as PooledObject;
        newObject.poolType = poolType;
        newObject.gameObject.name = poolType.ToString() + GameManager.Instance.createdPooledObjects;
        newObject.gameObject.SetActive(false);
        newObject.transform.SetParent(transform, false);
        targetPool.Enqueue(newObject);
        GameManager.AddCreations();

    }

    private Queue<PooledObject> GetQueue(ObjectPoolType type)
    {
        if (poolDictionary.ContainsKey(type) == false)
            return null;

        return poolDictionary[type];
    }

    private PooledObject GetPrefabFromPool(ObjectPoolType type)
    {
        int count = pools.Count;
        for (int i = 0; i < count; i++)
        {
            if (pools[i].type == type)
            {
                //if (variant != PoolVariantType.None)
                //{
                //    int vCount = pools[i].variants.Count;
                //    for (int j = 0; j < count; j++)
                //    {
                //        if(pools[i].variants[j].variant == variant)
                //        {
                //            return pools[i].variants[j].prefab;
                //        }
                //    }
                //}
                //else
                //{
                    return pools[i].prefab;
                //}
            }
        }

        return null;
    }


    public PooledObject GetObject(PoolRequestInfo info)
    {
        Queue<PooledObject> targetQueue = GetQueue(info.poolType);

        if (targetQueue == null)
            return null;

        if(targetQueue.Count == 0)
        {
            //Debug.LogError("--------------------Queue Empty, Should make more----------------------");
            CreateAndAddObject(info.poolType);
            //return null;
        }

        PooledObject obj = targetQueue.Dequeue();
        activeObjects.Add(obj);
        //Debug.Log("Getting " + obj.gameObject.name);

        
        Vector3 desiredPosition = info.position ?? Vector3.zero;
        if (info.localPosition)
            obj.transform.localPosition = desiredPosition;
        else
            obj.transform.position = desiredPosition;

        obj.transform.rotation = info.rotation ?? Quaternion.identity;
        obj.transform.SetParent(info.parent ?? transform, info.worldPositionStays);
        obj.OnGet();

        return obj;
    }

    public void ReturnObject(PooledObject obj)
    {
        if (obj == null)
        {
            Debug.Log("Object null, can't return it");
            return;
        }


        Queue<PooledObject> targetQueue = GetQueue(obj.poolType);

        if (targetQueue == null)
        {
            Debug.LogError("COuldn't find queue");
            return;
        }


        obj.transform.SetParent(transform, false);
        obj.OnReturn();
        if (activeObjects.Contains(obj))
            activeObjects.Remove(obj);
        targetQueue.Enqueue(obj);

    }


    public static PoolRequestInfo CreatePoolInfo(ObjectPoolType poolType, Transform parent = null, Vector3? position = null,
            Quaternion? rotation = null, bool worldPositionStays = false, bool localPosition = false)
    {
        return new PoolRequestInfo(poolType, parent, position, rotation, worldPositionStays, localPosition);
    }



    public struct PoolRequestInfo {
        public ObjectPoolType poolType;
        //public PoolVariantType variant;
        public Transform parent;
        public Vector3? position;
        public Quaternion? rotation;
        public bool worldPositionStays;
        public bool localPosition;

        public PoolRequestInfo(ObjectPoolType poolType, Transform parent = null, Vector3? position = null, 
            Quaternion? rotation = null, bool worldPositionStays = false, bool localPosition = false)
        {
            this.poolType = poolType;
            this.parent = parent;
            this.position = position;
            this.rotation = rotation;
            this.worldPositionStays = worldPositionStays;
            this.localPosition = localPosition;
            //this.variant = variant;
        }
    }


    public List<PooledObject> GetActiveObjectsWithinArea(Vector2 point, float radius)
    {
        List<PooledObject> results = new List<PooledObject>();
        int count = activeObjects.Count;
        for (int i = 0; i < count; i++)
        {
            if (Vector2.Distance(point, activeObjects[i].transform.position) < radius)
                results.Add(activeObjects[i]);
        }

        return results;
    }



}
