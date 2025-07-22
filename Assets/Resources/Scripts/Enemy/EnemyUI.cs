using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar, stmBar;
    [SerializeField] private TextMeshProUGUI txt;

    private void FixedUpdate() 
    {
        transform.localScale = transform.parent.localScale.x == -1 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void HpBarUpdate(float maxHp, float curHp)
    {
        hpBar.value = curHp / maxHp;
    }

    public void StmBarUpdate(float maxStm, float curStm)
    {
        stmBar.value = curStm / maxStm;
    }
}
