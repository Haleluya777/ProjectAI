using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using Unity.VisualScripting;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar, stmBar;
    [SerializeField] private Slider dashCoolDown, skillCoolDown_1;
    //[SerializeField] private TextMeshProUGUI txt;

    public void TextUpdate(string _text)
    {
        //txt.text = _text;
    }

    public void CheckSkillCoolDown(float remainingCoolDown, float coolDown)
    {
        if(remainingCoolDown == coolDown) return;
        skillCoolDown_1.value = remainingCoolDown / coolDown;
    }

    public void CheckDashCoolDown(float remainingCoolDown, float coolDown)
    {
        if(remainingCoolDown == coolDown) return;
        dashCoolDown.value = remainingCoolDown / coolDown;
    }

    public void HpBarUpdate(float maxHp, float curHp)
    {
        //if(maxHp == curHp) return;
        hpBar.value = curHp / maxHp;
    }

    public void StmBarUpdate(float maxStm, float curStm)
    {
        //if(maxStm == curStm) return;
        stmBar.value = curStm / maxStm;
    }
}
