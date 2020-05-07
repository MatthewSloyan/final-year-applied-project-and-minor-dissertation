using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    // Utilities method using generics. This is designed to allow any JSON to be converted to any object easily.
    public T ToObject<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    // Utilities method using generics. This is designed to allow any object to be converted to JSON easily.
    public string ToJsonString<T>(T obj)
    {
        return JsonUtility.ToJson(obj);
    }
}
