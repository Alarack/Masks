using System;
using System.Diagnostics;

/// <summary>
/// A tooltip for an enum member that EditorHelper.ShowEnumTooltip() can quickly access to display relevant help information.
/// This is essentially made for ease of adding inspector-shown documentation to (new) enum members.
/// This is for designers to quickly (one sentence or less) remind what the buff is for.
/// This is not for technical details, programmer info, or extended explanation.
/// </summary>
[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.Field)]
public class EnumTooltip : EnumAttribute
{
    public readonly string text;

    public EnumTooltip(string text)
    {
        this.text = text;
    }
}