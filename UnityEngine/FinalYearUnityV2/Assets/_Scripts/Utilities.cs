using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public T ToObject<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    public string ToJsonString<T>(T obj)
    {
        return JsonUtility.ToJson(obj);
    }
}
