using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour, IInitializable
{
    [SerializeField] private Slider hpBar, guardGage;
    int MaxHp, CurHp;
    float GuardGage, CurrentTime;

    private IBlackBoard localBlackBoard;

    private void FixedUpdate()
    {
        transform.localScale = transform.parent.localScale.x == -1 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void HpBarUpdate(float maxHp, float curHp)
    {
        if (maxHp == 0 || curHp == 0) return;
        hpBar.value = curHp / maxHp;
    }

    public void GuardGageUpdate(float _guardGage, float startTime)
    {
        if (_guardGage == 0 || startTime == 0) return;
        guardGage.value = (Time.time - startTime) / (_guardGage - startTime);
    }

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local)
    {
        localBlackBoard = local;
    }

    public void UpdateDataPerFrame(IBlackBoard local)
    {
        CurHp = local.Get<int>("CurHp");
        MaxHp = local.Get<int>("MaxHp");

        GuardGage = local.Get<float>("GuardGage");
        CurrentTime = local.Get<float>("CurrentTime");

        HpBarUpdate(MaxHp, CurHp);
        GuardGageUpdate(GuardGage, CurrentTime);
    }
}
