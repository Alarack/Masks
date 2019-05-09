using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour {

    public static StatusManager instance;

    public List<StatusEntry> statusEntries = new List<StatusEntry>();

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Initialize()
    {

    }

    private void Update()
    {
        for (int i = 0; i < statusEntries.Count; i++)
        {
            statusEntries[i].ManagedUpdate();
        }
    }

    public static void AddStatus(GameObject target, Status status)
    {
        int count = instance.statusEntries.Count;
        StatusEntry targetEntry = null;

        for (int i = 0; i < count; i++)
        {
            if (instance.statusEntries[i].target == target)
            {
                targetEntry = instance.statusEntries[i];
                break;
            }
        }

        if (targetEntry != null)
        {
            //Debug.Log("Status for " + target.gameObject.name + " exists");
            targetEntry.AddStatus(status);
            return;
        }

        //Debug.Log("Status for " + target.gameObject.name + " Does not exist!");
        StatusEntry newStatus = new StatusEntry(target, new StatusContainer(status));
        instance.statusEntries.Add(newStatus);

        if(string.IsNullOrEmpty(status.AnimBoolName) == false)
        {
            target.Entity().AnimHelper.PlayOrStopAnimBool(status.AnimBoolName, true);
        }
    }

    public static void RemoveStatus(GameObject target, Status targetStatus)
    {
        int count = instance.statusEntries.Count;
        StatusEntry targetEntry = null;

        for (int i = 0; i < count; i++)
        {
            if (instance.statusEntries[i].target == target)
            {
                targetEntry = instance.statusEntries[i];
                //statusManager.statusEntries.Remove(statusManager.statusEntries[i]);
                break;
            }
        }

        if (targetEntry != null)
        {
            targetEntry.RemoveStatus(targetStatus);

            if (string.IsNullOrEmpty(targetStatus.AnimBoolName) == false)
            {
                target.Entity().AnimHelper.PlayOrStopAnimBool(targetStatus.AnimBoolName, false);
            }

            if (targetEntry.GetStatusCount() < 1)
            {
                instance.statusEntries.Remove(targetEntry);
            }
        }
    }

    public static bool IsTargetAlreadyAffected(GameObject target, Status status)
    {
        int count = instance.statusEntries.Count;
        StatusEntry targetEntry = null;

        for (int i = 0; i < count; i++)
        {
            if (instance.statusEntries[i].target == target)
            {
                targetEntry = instance.statusEntries[i];
                //statusManager.statusEntries.Remove(statusManager.statusEntries[i]);
                break;
            }
        }

        if (targetEntry != null)
        {
            return targetEntry.IsTargetAlreadyAffected(target, status);
        }

        return false;

    }

    public static bool CheckForStatus(GameObject self, Constants.StatusType type)
    {
        bool result = false;

        List<Status> statusList = GetAllStatusOnTarget(self);

        if (statusList == null)
            return false;

        int count = statusList.Count;
        for (int i = 0; i < count; i++)
        {
            if (statusList[i].statusType == type)
            {
                result = true;
                break;
            }

        }


        return result;
    }

    private static List<Status> GetAllStatusOnTarget(GameObject target)
    {
        int count = instance.statusEntries.Count;

        for (int i = 0; i < count; i++)
        {
            if (instance.statusEntries[i].target == target)
            {
                return instance.statusEntries[i].GetStatusList();
            }
        }

        return null;
    }


    [System.Serializable]
    public class StatusEntry {
        public GameObject target;
        private StatusContainer statusContainer;

        public StatusEntry(GameObject target, StatusContainer statusContainer)
        {
            this.target = target;
            this.statusContainer = statusContainer;
        }

        public void ManagedUpdate()
        {
            statusContainer.ManagedUpdate();
        }

        public int GetStatusCount()
        {
            return statusContainer.activeStatusList.Count;
        }

        public bool IsTargetAlreadyAffected(GameObject target, Status status)
        {
            if (this.target != target)
                return false;

            List<Status> existingStatus = statusContainer.GetStatusListByType(status.statusType);
            int count = existingStatus.Count;

            if (count < 1)
                return false;

            for (int i = 0; i < count; i++)
            {
                if (existingStatus[i].IsFromSameSource(status.SourceAbility))
                {
                    return true;
                }
            }

            return false;
        }

        public List<Status> GetStatusList()
        {
            return statusContainer.activeStatusList;
        }


        public void AddStatus(Status status)
        {
            List<Status> existingStatus = statusContainer.GetStatusListByType(status.statusType);

            int count = existingStatus.Count;

            //Debug.Log(count + " existing status found of type " + status.statusType);

            if (existingStatus.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (StackStatus(status, existingStatus[i], status.SourceAbility) == true)
                    {
                        //Debug.Log("StatusManager: Stacking a status from " + status.SourceEffect.effectName + " on " + status.SourceAbility.abilityName);
                        return;
                    }

                }
            }

            //Debug.Log("StatusManager: Adding another status to the list from " + status.SourceEffect.effectName + " on " + status.SourceAbility.abilityName);
            statusContainer.AddStatus(status);
            status.FirstApply();
        }

        private bool StackStatus(Status status, Status existingStatus, Ability sourceAbility)
        {
            if (existingStatus.IsFromSameSource(sourceAbility))
            {
                switch (status.stackMethod)
                {
                    case Constants.EffectStackingMethod.None: //TODO: Move this out of the check for same source
                    case Constants.EffectStackingMethod.StacksWithOtherAbilities:
                        existingStatus.RefreshDuration();
                        return true;

                    case Constants.EffectStackingMethod.LimitedStacks:
                        if (existingStatus.StackCount < existingStatus.MaxStack)
                        {
                            existingStatus.Stack();
                        }
                        else
                        {
                            existingStatus.RefreshDuration();
                        }
                        return true;

                    case Constants.EffectStackingMethod.NewInstance:
                        return false;

                    case Constants.EffectStackingMethod.StacksInfinite:

                        existingStatus.Stack();
                        return true;
                }
            }

            return false;
        }

        public void RemoveStatus(Status status)
        {
            statusContainer.RemoveStatus(status);
        }

    }


    [System.Serializable]
    public class StatusContainer {
        public List<Status> activeStatusList = new List<Status>();

        public StatusContainer(Status initialStatus)
        {
            AddStatus(initialStatus);
            initialStatus.FirstApply();
        }

        public void AddStatus(Status status)
        {
            activeStatusList.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            if (activeStatusList.Contains(status))
            {
                activeStatusList.Remove(status);
            }
        }

        public void ManagedUpdate()
        {
            for (int i = 0; i < activeStatusList.Count; i++)
            {
                activeStatusList[i].ManagedUpdate();
            }
        }

        public List<Status> GetStatusListByType(Constants.StatusType type)
        {
            List<Status> results = new List<Status>();

            int count = activeStatusList.Count;

            for (int i = 0; i < count; i++)
            {
                if (activeStatusList[i].statusType == type)
                {
                    results.Add(activeStatusList[i]);
                }
            }

            return results;
        }


    }

}
