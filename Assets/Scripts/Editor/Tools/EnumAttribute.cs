using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

/// <summary>
/// Base class for custom attributes for enum members with common helper lookup methods.
/// </summary>
[Conditional("UNITY_EDITOR")]
public abstract class EnumAttribute : Attribute
{
    /// <summary>
    /// Get the attribute, if any, from the enum's member/value entry.
    /// In other words, if TEnum.value is marked [TAtt], then return that.
    /// </summary>
    public static TAtt GetAttribute<TAtt, TEnum>(TEnum enumValue, TEnum ignoreOnValue)
        where TAtt : EnumAttribute
        where TEnum : struct
    {
        if (Equals(enumValue, ignoreOnValue))
            return null;

        Type type = enumValue.GetType();

        MemberInfo memberInfo = type.GetMember(enumValue.ToString()).FirstOrDefault();
        if (memberInfo == null)
            return null;

        TAtt attribute = memberInfo.GetCustomAttributes(typeof(TAtt), false).FirstOrDefault() as TAtt;

        return attribute;
    }

    /// <summary>
    /// Get the attribute, if any, from the enum's member/value entry.
    /// In other words, if the enum of the given type is marked [TAtt], then return that.
    /// </summary>
    public static TAtt GetAttribute<TAtt>(Type type, string name)
        where TAtt : Attribute
    {
        MemberInfo memberInfo = type.GetMember(name).FirstOrDefault();
        if (memberInfo == null)
            return null;

        TAtt attribute = memberInfo.GetCustomAttributes(typeof(TAtt), false).FirstOrDefault() as TAtt;

        return attribute;
    }
}