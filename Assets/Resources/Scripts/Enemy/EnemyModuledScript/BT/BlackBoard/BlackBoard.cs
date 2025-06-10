using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    private Dictionary<string, object> data = new();

    public void Set<T>(string key, T value) => data[key] = value;

    public T Get<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;

    public bool HasKey(string key) => data.ContainsKey(key);

    public void Clear() => data.Clear();
}
