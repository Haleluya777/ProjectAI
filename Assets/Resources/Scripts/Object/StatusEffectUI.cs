using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class StatusEffectUI : MonoBehaviour
{
    //상태이상 받으면 체력바밑에 나오는 그거 오브젝트에 넣은 컴포넌트.
    //오브젝트는 프리팹으로 만들었고 오브젝트 풀링으로 하나씩 가져오게 함.

    public IObjectPool<GameObject> Pool { get; set; } //오브젝트 풀링하는거.

    private Slider slider;

    public float remainingDuration, duration;  //남은 지속시간, 지속시간.

    private void Awake() //별거 없음.
    {
        slider = this.GetComponent<Slider>();    
    }

    private void Update() //마찬가지지
    {
        remainingDuration -= Time.deltaTime;
        StatusEffectRemainigTime();
    }

    public void SetVariable(float _remainingDuration, float _duration) //프리팹 오브젝트가 생성될때 한번 실행됨. 이건 PlayerController쪽에서 제어함.
    {
        remainingDuration = _remainingDuration;
        duration = _duration;
    }

    public void StatusEffectRemainigTime() //그냥 남은시간 계산하는거.
    {
        if(remainingDuration <= 0)
        {
            GetBackToPool();
            return;
        }
        slider.value = remainingDuration / duration;
    }

    public void GetBackToPool() //시간 다되면 오브젝트 풀로 돌아가게 하는거.
    {
        GameManager.instance.objectPoolManger_EffectTime.Pool.Release(this.gameObject);
    }
}
