using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySTATS : MonoBehaviour
{
    [Header("Assignable Attributes")]
    public float baseSpeed = 10f;
    public float baseDamage = 20f;
    public int attackCombos = 3;
    public float jumpStrength = 2f;
    [Header("Attribute Modifiers (DEBUG)")]
    public float damageModifier = 1;
    public float speedModifier = 1;
    public float curSpeed;
    public float curDamage;
    public int curAttackCombo;
    //[Header("Assign In Inspector")]

    void Update(){
        curSpeed = baseSpeed * speedModifier;
        curDamage = baseDamage * damageModifier;
    }
}
