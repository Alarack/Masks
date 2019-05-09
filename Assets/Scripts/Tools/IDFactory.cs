
public class IDFactory {

    public static int EntityID;
    public static int EffectID;
    public static int AbilityID;


    public static int GenerateEntityID()
    {
        EntityID++;
        return EntityID;
    }

    public static int GenerateEffectID()
    {
        EffectID++;
        return EffectID;
    }

    public static int GenerateAbilityID()
    {
        AbilityID++;
        return AbilityID;
    }

}
