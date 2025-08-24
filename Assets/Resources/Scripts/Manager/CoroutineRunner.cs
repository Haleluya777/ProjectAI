using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    public Coroutine StartRunnerCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
