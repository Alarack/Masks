using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectZoneFactory {


    public static EffectZone CreateEffect(ZoneInfo zoneInfo, Constants.EffectOrigin location, GameObject source)
    {
        EffectZone result = null;

        Vector3 spawnPoint;

        if(location != Constants.EffectOrigin.MousePointer)
        {
            Transform point = source.Entity().EffectDelivery.GetOriginPoint(location);
            spawnPoint = point.position;
        }
        else
        {
            Debug.LogError("Add Raycasting for MouseLocation effect creation");
            spawnPoint = source.transform.position;
        }

        GameObject zoneObject = LoadAndSpawnZonePrefab(zoneInfo, spawnPoint, Quaternion.identity);

        if (zoneObject == null)
            return null;

        EntityMovement.FacingDirection facing = source.Entity().Movement.Facing;

        if (facing == EntityMovement.FacingDirection.Left)
        {
            zoneObject.transform.localScale = new Vector3(zoneObject.transform.localScale.x * -1, zoneObject.transform.localScale.y, zoneObject.transform.localScale.z);
        }


        result = ConfigureZone(zoneInfo, ref zoneObject, ref result);

        return result;
    }

    public static EffectZone CreateEffect(ZoneInfo zoneInfo, Vector3 spawnPoint, Quaternion spawnRotation)
    {
        EffectZone result = null;
        GameObject zoneObject = LoadAndSpawnZonePrefab(zoneInfo, spawnPoint, spawnRotation);

        if (zoneObject == null)
            return null;

        result = ConfigureZone(zoneInfo, ref zoneObject, ref result);

        return result;
    }


    private static EffectZone ConfigureZone(ZoneInfo zoneInfo, ref GameObject zoneObject, ref EffectZone result)
    {
        switch (zoneInfo.durationType)
        {
            case EffectZone.EffectZoneDuration.Instant:
                result = zoneObject.AddComponent<EffectZoneInstant>();
                break;

            case EffectZone.EffectZoneDuration.Persistant:
                result = zoneObject.AddComponent<EffectZonePersistant>();
                ((EffectZonePersistant)result).InitializePersistantZone(zoneInfo.duration, zoneInfo.interval, zoneInfo.removeEffectOnExit);
                break;
        }

        return result;
    }



    private static GameObject LoadAndSpawnZonePrefab(ZoneInfo zoneInfo, Vector3 spawnPoint, Quaternion spawnRotation)
    {
        GameObject loadedPrefab = VisualEffectLoader.LoadVisualEffect(zoneInfo.shape, zoneInfo.size, zoneInfo.zoneName);

        if (loadedPrefab == null)
        {
            Debug.LogError("Could not load zone prefab with " + zoneInfo.shape + " " + zoneInfo.size + " " + zoneInfo.zoneName);
            return null;
        }

        return SpawnZonePrefab(loadedPrefab, spawnPoint, spawnRotation);
    }

    private static GameObject SpawnZonePrefab(GameObject prefab, Vector3 spawnPoint, Quaternion spawnRotation)
    {
        return GameObject.Instantiate(prefab, spawnPoint, spawnRotation) as GameObject;
    }

}
