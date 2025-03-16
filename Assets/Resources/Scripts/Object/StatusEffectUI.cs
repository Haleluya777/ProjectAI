using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class StatusEffectUI : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    private Slider slider;

    public float remainingDuration, duration; 

    private void Awake() 
    {
        slider = this.GetComponent<Slider>();    
    }

    private void Update() 
    {
        remainingDuration -= Time.deltaTime;
        StatusEffectRemainigTime();
    }

    public void SetVariable(float _remainingDuration, float _duration)
    {
        remainingDuration = _remainingDuration;
        duration = _duration;
    }

    public void StatusEffectRemainigTime()
    {
        if(remainingDuration <= 0)
        {
            GetBackToPool();
            return;
        }
        slider.value = remainingDuration / duration;
    }

    public void GetBackToPool()
    {
        GameManager.instance.objectPoolManger1.Pool.Release(this.gameObject);
    }
}
