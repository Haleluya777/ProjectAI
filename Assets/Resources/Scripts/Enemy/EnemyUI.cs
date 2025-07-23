using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour, IInitializable
{
    [SerializeField] private Slider hpBar, guardGage;
    [SerializeField] private TextMeshProUGUI txt;

    private IBlackBoard localBlackBoard;

    private void FixedUpdate()
    {
        transform.localScale = transform.parent.localScale.x == -1 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void HpBarUpdate(float maxHp, float curHp)
    {
        hpBar.value = curHp / maxHp;
    }

    public void GuardGageUpdate(float _guardGage, float startTime)
    {
        //if (!localBlackBoard.HasKey("GuardGage"))
        //{
        //    guardGage.value = 0;
        //    return;
        //}
        guardGage.value = (Time.time - startTime) / (_guardGage - startTime);
    }

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local)
    {
        localBlackBoard = local;
    }

    public void UpdateDataPerFrame(IBlackBoard local)
    {
        HpBarUpdate(local.Get<int>("MaxHp"), local.Get<int>("CurHp"));
        GuardGageUpdate(local.Get<float>("GuardGage"), local.Get<float>("CurrentTime"));
    }
}
