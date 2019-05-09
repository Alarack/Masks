using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stat Set")]
[System.Serializable]
public class StatCollectionData : ScriptableObject {

    public string collectionName;

    public List<StatDataDisplay> stats = new List<StatDataDisplay>();



    public void CreateStatsFromData(StatCollection collection)
    {
        int count = stats.Count;
        for (int i = 0; i < count; i++)
        {
            StatDataDisplay currentDisplay = stats[i];

            switch (currentDisplay.variant)
            {
                case BaseStat.StatVariant.Capped:
                    //Debug.Log(currentDisplay.statType + " is capped");
                    collection.AddStat(currentDisplay.statType, currentDisplay.baseValue, currentDisplay.baseValue);
                    break;

                case BaseStat.StatVariant.Simple:
                    //Debug.Log(currentDisplay.statType + " is simple");
                    collection.AddStat(currentDisplay.statType, currentDisplay.baseValue);
                    break;
            }
        }
    }


    [System.Serializable]
    public class StatDataDisplay {
        public BaseStat.StatVariant variant;
        public BaseStat.StatType statType;
        public float baseValue;
    }

}
