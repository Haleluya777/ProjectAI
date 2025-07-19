using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlackBoard
{
    void Set<T>(string key, T value);
    T Get<T>(string key);
    bool HasKey(string key);
    void Clear();
    void Remove(string key);
}
