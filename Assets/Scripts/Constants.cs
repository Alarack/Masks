using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants  {


    public enum GameEvent {
        None = 0,
        StatChanged = 1,
        EntityDied = 2,
        EffectApplied = 3,
        EffectRemoved = 4,
        AnimEvent = 5,

    }

    public enum EffectTag {
        None = 0,
        DirectDamage,
        PeriodicDamage,
        DirectHealing,
        PeriodicHealing,
        Ranged,
        Melee,
        Area,
        Bleed,
        Burn,
        ForcedMovement,
        Projectile,
        Summon,
        Aura,
        Curse,
        Movement,
        Freeze,
        Poison,
        Primary,
        Air,
        Earth,
        Water,
        Fire,
        Darkness,
        Light,
        Life,
        Death,
        Holy,
        Unholy,
        Chaos,
        Order,
        Time,
        Space,
        Void,
        Force,
        Buff

    }

    public enum AbilityActivationMethod {
        None = 0,
        Manual = 1,
        Timed = 2,
        StatChanged = 3,
        Passive = 4,
        EntityKilled = 5,
        EffectApplied = 6,
        EffectRemoved = 7,
        AbilityActivated = 8,
    }

    public enum AbilityRecoveryType {
        None = 0,
        Cooldown = 1,
        Kills = 2,

    }

    public enum AbilitySourceType {
        Entity = 0,
        Projectile = 1,
    }

    public enum AbilityActivationCondition {
        Normal = 0,
        IgnoreCost = 1,
        IgnoreRecovery = 2
    }

    public enum EffectOrigin {
        None = 0,
        LeftHand = 1,
        RightHand = 2,
        CharacterCenter = 3,
        MousePointer = 4,
        CharacterFront = 5,
        Head = 6
    }

    public enum EffectDeliveryMethod {
        None = 0,
        Instant = 1,
        Projectile = 2,
        SelfTargeting = 3,
        ExistingTargets = 4,
        Rider = 5,
    }

    public enum EffectType {
        None = 0,
        StatAdjustment = 1,
        AddForce = 2
    }

    public enum StatusType {
        None = 0,
        Stun = 1,
        Burn = 2,
        Freeze = 3,
        Blind = 4,
        Confuse = 5,
        Slow = 6,
        Poison = 7,
        Immobilize = 8,
        MovementAffecting = 9,
        ForceMaxSpeed = 10,
        Knockback = 11

    }

    public enum EffectStackingMethod {
        None = 0,
        LimitedStacks = 1,
        StacksWithOtherAbilities = 2,
        NewInstance = 3,
        StacksInfinite = 4

    }

    public enum EffectSourceType {
        None = 0,
        Entity = 1,
        Projectile = 2,
    }

    public enum EffectDurationType {
        Instant = 0,
        Duration = 1,
        Periodic = 2,
    }



}
