using System;
using System.Diagnostics;

/// <summary>
/// Specifies that when an enum's member is to be drawn through custom EditorHelper.EnumPopup(),
/// it should be grouped in the context menu as specified by "group" (use "/" to sub-group).
/// Use this to organize/categorize otherwise unwieldly enums.
/// </summary>
[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.Field)]
public class EnumGroup : EnumAttribute
{
    /// <summary>
    /// The group (sub-group) that the enum memeber will be under.
    /// For example "Items" or "Items/Swords". (Regular enum member name will still be added)
    /// </summary>
    public readonly string group;

    public EnumGroup(string group)
    {
        this.group = group;
    }
}