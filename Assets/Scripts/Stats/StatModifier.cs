using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier {

    public enum StatModificationType {
        Additive,
        Multiplicative
    }

    public float Value { get; private set; }
    public StatModificationType ModType { get; private set; }

    public StatModifier(float value, StatModificationType modType)
    {
        this.Value = value;
        this.ModType = modType;
    }

    public float GetValueByModType(StatModificationType type)
    {

        //Debug.Log("getting a value for type " + type);

        //if (ModType == StatModificationType.Multiplicative)
        //    Debug.Log(Value + " is a muli mod value");

        //if (ModType == StatModificationType.Additive)
        //    Debug.Log(Value + " is an add mod value");

        return type == ModType ? Value : 0f;
    }

}
