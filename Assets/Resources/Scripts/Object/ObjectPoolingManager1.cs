using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolingManager1 : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; private set; }
    public GameObject pooledObj;

    public int defaultCapacity = 10;
    public int maxSize = 20;

    private void Awake() 
    {
        Pool = new ObjectPool<GameObject>(CreatePooledObj, TakeFromPool, ReturnToPool, DestroyPooledObj, true, defaultCapacity, maxSize);
        ObjectPoolInit();
    }

    private void ObjectPoolInit()
    {
        for(int i = 0; i < defaultCapacity; i++)
        {
            StatusEffectUI statusEffectUI = CreatePooledObj().GetComponent<StatusEffectUI>();
            statusEffectUI.Pool.Release(statusEffectUI.gameObject);
        }
    }

    private GameObject CreatePooledObj()
    {
        var obj = Instantiate(pooledObj);
        obj.GetComponent<StatusEffectUI>().Pool = this.Pool;
        obj.transform.SetParent(this.transform);
        return obj;
    }

    private void TakeFromPool(GameObject pooledObj)
    {
        pooledObj.SetActive(true);
    }

    private void ReturnToPool(GameObject pooledObj)
    {
        pooledObj.SetActive(false);
        pooledObj.transform.SetParent(this.transform);
    }

    private void DestroyPooledObj(GameObject pooledObj)
    {
        Destroy(pooledObj);
    }
}
