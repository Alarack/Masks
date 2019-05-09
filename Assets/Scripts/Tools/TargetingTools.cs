using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TargetingTools
{


    public static GameObject FindNearestTarget(List<GameObject> targets, Transform myTransform)
    {
        GameObject result = null;

        Dictionary<GameObject, float> distances = new Dictionary<GameObject, float>();

        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            float distance = Vector2.Distance(myTransform.position, targets[i].transform.position);
            distances.Add(targets[i], distance);
        }

        result = distances.OrderBy(k => k.Value).First().Key;
        //GameObject key = distances.min(kvp => kvp.Value)

        return result;
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }




}
