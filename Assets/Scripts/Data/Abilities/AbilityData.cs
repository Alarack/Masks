using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectTag = Constants.EffectTag;
using AbilityActivationMethod = Constants.AbilityActivationMethod;

[CreateAssetMenu(menuName = "Ability Data")]
[System.Serializable]
public class AbilityData : ScriptableObject {

    public string abilityName;
    public float baseWeight;
    //public List<AbilityActivationMethod> triggers = new List<AbilityActivationMethod>();
    public List<AbilityActivationInfo> activations = new List<AbilityActivationInfo>();
    public List<AbilityCondition> conditions = new List<AbilityCondition>();

    public Sprite abilityIcon;
    public float useDuration;
    public float procChance = 1f;
    public bool overrideOtherAbilities;

    public List<EffectData> effectData = new List<EffectData>();
    public List<RecoveryData> recoveryData = new List<RecoveryData>();

    public List<AbilityData> sequencedAbilities = new List<AbilityData>();
    public float sequenceWindow = 0.5f;

    //public List<EffectTag> tags = new List<EffectTag>();

}
