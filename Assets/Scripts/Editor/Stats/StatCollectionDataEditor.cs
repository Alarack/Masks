using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StatCollectionData))]
public class StatCollectionDataEditor : Editor {

    private StatCollectionData _StatData;


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _StatData = (StatCollectionData)target;

        _StatData.collectionName = EditorGUILayout.TextField("Stat Template Name", _StatData.collectionName);

        EditorGUILayout.Separator();

        _StatData.stats = EditorHelper.DrawExtendedList("Stat Collection", _StatData.stats, "Stat", DrawStatDisplay);


        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }



    private StatCollectionData.StatDataDisplay DrawStatDisplay(StatCollectionData.StatDataDisplay entry)
    {
        entry.variant = EditorHelper.EnumPopup("Stat Variant", entry.variant);

        if (entry.variant == BaseStat.StatVariant.None)
            return entry;

        entry.statType = EditorHelper.EnumPopup("Stat Type", entry.statType);

        switch (entry.statType)
        {
            case BaseStat.StatType.Health:
                entry.baseValue = EditorHelper.FloatField("Health", entry.baseValue);
                break;

            case BaseStat.StatType.CritChance:
                entry.baseValue = EditorHelper.PercentFloatField("Crit Chance", entry.baseValue);
                break;

            case BaseStat.StatType.CritMultiplier:
                entry.baseValue = EditorHelper.PercentFloatField("Crit Damage", entry.baseValue);
                break;

            case BaseStat.StatType.AttackSpeed:
                entry.baseValue = EditorHelper.PercentFloatField("Attack Speed", entry.baseValue);
                break;

            case BaseStat.StatType.Lifetime:
                entry.baseValue = EditorHelper.FloatField("Lifetime", entry.baseValue);
                break;

            case BaseStat.StatType.MoveSpeed:
                entry.baseValue = EditorHelper.FloatField("Move Speed", entry.baseValue);
                break;

            case BaseStat.StatType.RotateSpeed:
                entry.baseValue = EditorHelper.FloatField("Turn Speed", entry.baseValue);
                break;

            case BaseStat.StatType.BaseDamage:
                entry.baseValue = EditorHelper.FloatField("Base Damage", entry.baseValue) * -1;
                break;

            case BaseStat.StatType.DamageReduction:
                entry.baseValue = EditorHelper.PercentFloatField("Damage Reduction", entry.baseValue);
                break;

            case BaseStat.StatType.AbilityCharge:
                entry.baseValue = EditorHelper.FloatField("Charges", entry.baseValue);
                break;

            case BaseStat.StatType.ProjectilePenetration:
                entry.baseValue = EditorHelper.IntField("Penetration Count", (int)entry.baseValue);
                break;
        }




        return entry;
    }


}
