using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using Unity.VisualScripting;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectUI;
    [SerializeField] private Slider hpBar, stmBar;
    [SerializeField] private Slider dashCoolDown, skillCoolDown_1;
    //[SerializeField] private List<GameObject> effectList = new List<GameObject>();
    private Dictionary<string, GameObject> effectList = new Dictionary<string, GameObject>();
    //[SerializeField] private TextMeshProUGUI txt;


    private void Update()
    {
        //Debug.Log(effectList.Count);
    }

    //상태이상의 남은 시간을 알려주는 Slider를 Prefab으로 만든 후 ObjectPool에서 가져오는 방식으로 구현.
    //작동은 완벽하나, 생성된 Prefab의 위치를 완벽하게 조정하지 못하고 있는 중.
    public void CreateEffectUISlider(StatusEffect effect)
    {
        var obj = GameManager.instance.objectPoolManger1.Pool.Get(); //오브젝트 풀에서 오브젝트를 빌려옴.

        obj.transform.SetParent(statusEffectUI.transform); //부모 조정.  
        obj.GetComponent<StatusEffectUI>().SetVariable(effect.duration, effect.duration);
        effectList.Add(effect.effectName, obj);
        //obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(50 + (150 * (activeEffect.Count - 1)) ,1);
    }

    public void RenewalEffectSlider(StatusEffect effect, string effectName)
    {
        effectList[effectName].gameObject.GetComponent<StatusEffectUI>().SetVariable(effect.duration, effect.duration);
    }

    public void RemoveEffectUI(string effectName)
    {
        effectList.Remove(effectName);
    }

    //캔버스에 보이는 상태 이상 관련 UI를 업데이트 하는 함수.
    public void UpdateEffectUI()
    {
        int i = 0;
        foreach(string name in effectList.Keys)
        {
            effectList[name].GetComponent<RectTransform>().anchoredPosition = new Vector2(50 + (150 * i) ,1);
            i++;
        }
    }

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
