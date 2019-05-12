using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public DimensionMode dimensionMode;
    public static GameManager Instance;
    public Transform splatHolder;
    public SpawnManager spawnManager;
    public ObjectPoolManager objectPools;

    public int createdPooledObjects;

    public static List<Ability> allAbilities = new List<Ability>();
    public static List<Entity> allEntities = new List<Entity>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        spawnManager = GetComponentInChildren<SpawnManager>();
        objectPools = GetComponentInChildren<ObjectPoolManager>();

    }

    private void Start()
    {
        int count = allEntities.Count;
        for (int i = 0; i < count; i++)
        {
            allEntities[i].Initialize();
        }
    }

    public static void RegisterEntity(Entity entity)
    {
        allEntities.AddUnique(entity);
    }

    public static Ability GetAbility(string abilityName)
    {
        //Debug.Log("Searcing for " + abilityName);

        int count = allAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (allAbilities[i].abilityName == abilityName)
                return allAbilities[i];
        }

        //Debug.Log("couldin't find " + abilityName);

        return null;
    }

    public static Ability GetAbilityByOwner(GameObject owner, string abilityName)
    {
        int count = allAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (allAbilities[i].abilityName == abilityName && allAbilities[i].Source == owner)
                return allAbilities[i];
        }

        Debug.Log("couldin't find " + abilityName);

        return null;
    }

    public static Ability GetAbility(int id)
    {
        int count = allAbilities.Count;
        for (int i = 0; i < count; i++)
        {
            if (allAbilities[i].AbilityID == id)
                return allAbilities[i];
        }

        return null;
    }
    
    public static void RegisterAbility(Ability ability)
    {
        allAbilities.AddUnique(ability);
    }


    public static void AddCreations()
    {
        Instance.createdPooledObjects++;
    }

    public static PooledObject GetPooledObject(ObjectPoolManager.PoolRequestInfo info)
    {
        return Instance.objectPools.GetObject(info);
    }

    public static void ReturnPooledObject(PooledObject obj)
    {
        Instance.objectPools.ReturnObject(obj);
    }

}
