using System.Collections.Generic;

public static class IListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    /// <summary>
    /// Adds an object to a list, but only if it was not already in that list. Returns True if successful.
    /// </summary>
    public static bool AddUnique<T>(this IList<T> list, T entry)
    {
        if (list.Contains(entry) == false)
        {
            list.Add(entry);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes an object from a list, but only if it was contained in that list. Returns True if successful.
    /// </summary>
    public static bool RemoveIfContains<T>(this IList<T> list, T entry)
    {
        if (list.Contains(entry) == true)
        {
            list.Remove(entry);
            return true;
        }

        return false;
    }
}