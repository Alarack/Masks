using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileSpreadType = ProjectileInfo.ProjectileSpreadType;

public static class ProjectileFactory {



    public static Projectile CreateProjectile(ProjectileInfo info, Constants.EffectOrigin location, GameObject source, int currentProjectile = 0)
    {
        Vector3 spawnPoint;
        Quaternion spawnRot = Quaternion.identity;

        if (location != Constants.EffectOrigin.MousePointer)
        {
            Transform point = source.Entity().EffectDelivery.GetOriginPoint(location);
            spawnPoint = point.position;
            spawnRot = point.rotation;
        }
        else
        {
            Debug.Log("Add in mouse position projectiles spawns");
            spawnPoint = source.transform.position;
        }

        Projectile loadedProjectile = LoadAndSpawnProjectile(info.prefabName, spawnPoint, spawnRot);

        if (loadedProjectile == null)
            return null;

        return ConfigureProjectile(info, ref loadedProjectile, currentProjectile);
    }

    private static Projectile ConfigureProjectile(ProjectileInfo info, ref Projectile projectile, int currentProjectile = 0)
    {
        switch (info.spreadType)
        {
            case ProjectileSpreadType.None:

                break;

            case ProjectileSpreadType.Random:
                float error = Random.Range(info.error, -info.error);


                projectile.transform.rotation = Quaternion.Euler(
                    projectile.transform.rotation.x, 
                    projectile.transform.rotation.y, 
                    error);


                //projectile.transform.rotation = Quaternion.Euler(projectile.transform.rotation.x, error, projectile.transform.rotation.z);
                break;

            case ProjectileSpreadType.EvenlySpread:

                float errorRange = info.error;
                float percentOfRange = info.projectileCount / errorRange;

                float newOffset = /*(currentProjectile + 1) +*/ info.error /** percentOfRange*/;
                if (currentProjectile == 0 && info.projectileCount.IsOdd())
                {
                    newOffset = 0f;
                }

                if (currentProjectile.IsOdd())
                {
                    //newOffset = (currentProjectile - 1) + info.error;
                    newOffset *= -1f;
                }


                float offset = currentProjectile == 0 && info.projectileCount.IsOdd() ? 0 : (currentProjectile) * percentOfRange;

                Debug.Log((newOffset)+ " is the current offset");

                projectile.transform.rotation = Quaternion.Euler(
                    projectile.transform.rotation.x,
                    projectile.transform.rotation.y, 
                    newOffset);


                //projectile.transform.rotation = Quaternion.Euler(projectile.transform.rotation.x, offset, projectile.transform.rotation.z);

                break;
        }

        return projectile;
    }

    private static Projectile LoadAndSpawnProjectile(string prefabName, Vector3 spawnPoint, Quaternion spawnRotation)
    {
        //Projectile loadedPrefab = Resources.Load("Projectiles/" + prefabName) as Projectile;

        GameObject loadedPrefab = Resources.Load("Projectiles/" + prefabName) as GameObject;

        if (loadedPrefab == null)
        {
            Debug.LogError("Could not load projectile: " + prefabName);
            return null;
        }

        return SpawnProjectile(loadedPrefab.GetComponent<Projectile>(), spawnPoint, spawnRotation);

    }

    private static Projectile SpawnProjectile(Projectile prefab, Vector3 spawnPoint, Quaternion spawnRotation)
    {
        return GameObject.Instantiate(prefab, spawnPoint, spawnRotation) as Projectile;
    }

}


[System.Serializable]
public struct ProjectileInfo {

    public enum ProjectileSpreadType {
        None = 0,
        Random = 1,
        EvenlySpread = 2,
    }


    public string prefabName;
    public ProjectileSpreadType spreadType;
    public int projectileCount;
    public float error;
    public float burstDelay;
    public bool addInitialForce;
    public AddForceInfo initialForce;


    public ProjectileInfo(string prefabName, ProjectileSpreadType spreadType, int projectileCount, float error, float burstDelay,
        bool addInitialForce, AddForceInfo? initialForce = null)
    {
        this.prefabName = prefabName;
        this.spreadType = spreadType;
        this.projectileCount = projectileCount;
        this.error = error;
        this.burstDelay = burstDelay;
        this.addInitialForce = addInitialForce;
        this.initialForce = initialForce ?? new AddForceInfo();

    }

}
