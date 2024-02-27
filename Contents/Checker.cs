using System.Linq;
using UnityEngine;
public static class Checker
{
    public static bool Exist(params Object[] parameters)
    {
        return parameters.All(x => x != null);
    }
    public static bool AnyParametersNull(params Object[] parameters)
    {
        foreach (Object param in parameters)
        {
            if (param == null)
            {
                return true;
            }
        }
        return false;
    }
    public static T RequireComponent<T>(GameObject go, int index = 0) where T : UnityEngine.Component
    {
        var arr = go.GetComponents<T>();
        T component = null;
        if (arr == null || index >= arr.Length) component = go.AddComponent<T>();
        else component = go.GetComponents<T>()[index];

        return component;
    }
}


