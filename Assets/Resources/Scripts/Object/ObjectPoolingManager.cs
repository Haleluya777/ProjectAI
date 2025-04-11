using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolingManager : MonoBehaviour
{
    //오브젝트 풀링 매니저가 두 개 있는데 나중에 통합해야 함.
    //하나는 Enemy가 데미지 받을 때 텍스트 출력하는 거고 나머지 하나는 상태이상 ui생성하는거.
    //https://starlightbox.tistory.com/84 이거 참고.
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
            DmgText dmgText = CreatePooledObj().GetComponent<DmgText>();
            dmgText.Pool.Release(dmgText.gameObject);
        }
    }

    private GameObject CreatePooledObj()
    {
        var obj = Instantiate(pooledObj);
        obj.GetComponent<DmgText>().Pool = this.Pool;
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
