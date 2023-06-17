using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiscUtils
{
    public static void ForEach<T>(this T[] arr, Action<T> action) 
    { 
        foreach(var item in arr) 
            action(item); 
    }

    public static T GetLastItem<T>(this List<T> lis)
    {
        if(lis.Count == 0) return default(T);
        return lis[lis.Count - 1];
    }

    public static bool IsVectorNeighbour(this Vector2 v, Vector2 w)
    {
        Vector3 difference = new Vector3(Mathf.Abs(v.x - w.x), Mathf.Abs(v.y - w.y));
        return (difference.x == 1 && difference.y == 0) || (difference.x == 0 && difference.y == 1);
    }
}
