using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public GameObject playerObj;

    [SerializeField] public ObjectPoolingManager objectPoolManger;
    [SerializeField] public ObjectPoolingManager1 objectPoolManger1;

    [SerializeField] private bool inBattle = false;

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
}
