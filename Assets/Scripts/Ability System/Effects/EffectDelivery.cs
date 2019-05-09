using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDelivery : MonoBehaviour {

    public List<EffectOriginPoint> effectOrigins = new List<EffectOriginPoint>();


    private Entity owner;


    private void Awake()
    {
        owner = GetComponentInParent<Entity>();
    }



    public Transform GetFront()
    {
        Transform left = GetOriginPoint(Constants.EffectOrigin.LeftHand);
        Transform right = GetOriginPoint(Constants.EffectOrigin.RightHand);


        return owner.Movement.Facing == EntityMovement.FacingDirection.Left ? left : right;
    }

    public Transform GetOriginPoint(Constants.EffectOrigin originType)
    {
        if (originType == Constants.EffectOrigin.CharacterFront)
            return GetFront();

        int count = effectOrigins.Count;
        for (int i = 0; i < count; i++)
        {
            if (effectOrigins[i].originType == originType)
                return effectOrigins[i].point;
        }

        return null;
    }

    public EffectOriginPoint? GetCurrentWeaponPoint()
    {
        if (owner.CurrentWeapon == null)
            return null;

        Transform currentPoint = owner.CurrentWeapon.transform.parent;

        int count = effectOrigins.Count;
        for (int i = 0; i < count; i++)
        {
            if (effectOrigins[i].point == currentPoint)
                return effectOrigins[i];
        }

        return null;

    }

   

}
