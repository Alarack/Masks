using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor : MonoBehaviour {

    public float detectionDistance = 15f;
    public LayerMask targetLayers;
    public float forgetDistance = 20f;
    //public LayerMask seeableLayers;
    [Space(10)]
    public List<GameObject> targets = new List<GameObject>();

    private EntityEnemy owner;
    private CircleCollider2D circleCollider;

    public GameObject ClosestTarget { get; private set; }

    public GameObject targetIcon;

    public void Initialize(EntityEnemy owner)
    {
        this.owner = owner;
        circleCollider = GetComponent<CircleCollider2D>();
    }


    private void Update()
    {
        RemoveFarTargets();
        RemoveOutOfSightTargets();

        if (targets.Count > 0)
            ClosestTarget = GetClosestTarget();

        //if (ClosestTarget != null)
        //    Debug.Log(ClosestTarget.name + " is closest");

        //SetTargetIcon();
    }

    private void RemoveFarTargets()
    {
        int count = targets.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            if (Vector2.Distance(transform.position, targets[i].transform.position) > forgetDistance)
            {
                Debug.Log("forgetting " + targets[i].name);
                if (ClosestTarget != null && ClosestTarget == targets[i])
                    ClosestTarget = null;

                RemoveTarget(i);
            }
        }
    }

    private void RemoveOutOfSightTargets()
    {
        int count = targets.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            GameObject target = TryToDetect(targets[i]);

            if (target == null)
            {
                if (ClosestTarget != null && ClosestTarget == targets[i])
                    ClosestTarget = null;

                RemoveTarget(i);

                //Debug.Log(targets[i].gameObject.name + " is out of sight");
            }
        }
    }

    private void SetTargetIcon()
    {
        if (targets.Count < 1 && targetIcon.activeSelf == true)
            targetIcon.SetActive(false);
        else if (ClosestTarget != null)
        {
            if (targetIcon.activeSelf == false)
                targetIcon.SetActive(true);
            targetIcon.transform.position = ClosestTarget.transform.position;
        }
    }

    private GameObject GetClosestTarget()
    {
        if (targets.Count == 1)
        {
            return targets[0];
        }

        return TargetingTools.FindNearestTarget(targets, transform);
    }

    private GameObject TryToDetect(GameObject other)
    {
        Vector2 direction = other.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, detectionDistance);

        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name + " is seen");
            if (LayerTools.IsLayerInMask(targetLayers, hit.collider.gameObject.layer) == true)
            {
                //Debug.Log(hit.collider.gameObject.name + " is seen");
                return hit.collider.gameObject;
            }

        }

        return null;

    }

    private void RemoveTarget(GameObject target)
    {
        targets.RemoveIfContains(target);
    }

    private void RemoveTarget(int index)
    {
        if(index < targets.Count + 1)
        {
            targets.RemoveAt(index);
        }

        if(targets.Count < 1)
        {
            owner.Brain.NoTargets();
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (LayerTools.IsLayerInMask(targetLayers, other.gameObject.layer) == false)
            return;

        GameObject target = TryToDetect(other.gameObject);
        if (target != null)
        {
            if (targets.AddUnique(target))
            {
                owner.Brain.TargetSpotted();
            }

            //if (targets.AddUnique(target))
            //{
            //    ((EntityEnemy)owner).Aggro();
            //}
        }

    }



}
