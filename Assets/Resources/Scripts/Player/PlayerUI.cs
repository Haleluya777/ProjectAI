using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar, stmBar;
    [SerializeField] private TextMeshProUGUI txt;

    public void TextUpdate(string _text)
    {
        txt.text = _text;
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
