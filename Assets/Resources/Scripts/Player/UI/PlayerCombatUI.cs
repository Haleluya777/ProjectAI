using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatUI : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectUI;
    [SerializeField] private Slider hpBar, stmBar;
    [SerializeField] private Slider dashCoolDown;
    [SerializeField] private List<Slider> skillCoolDown = new List<Slider>();
    [SerializeField] private List<GameObject> effectList = new List<GameObject>();
    private Dictionary<string, GameObject> effectDic = new Dictionary<string, GameObject>();
    //[SerializeField] private TextMeshProUGUI txt;


    //상태이상의 남은 시간을 알려주는 Slider를 Prefab으로 만든 후 ObjectPool에서 가져오는 방식으로 구현.
    //작동은 완벽하나, 생성된 Prefab의 위치를 완벽하게 조정하지 못하고 있는 중.
    //조정 완료.

    //작동 원리.
    //PlayerUi의 딕셔너리에 모든 종류의 상태 이상 정보를 넣어둔 후, 상태 이상에 걸릴 때마다 딕셔너리에서 필요한 값을 빼온 후 ui에 적용시키는 방식.
    public void CreateEffectUISlider(StatusEffect effect)
    {
        var obj = GameManager.instance.objectPoolManger_EffectTime.Pool.Get(); //오브젝트 풀에서 오브젝트를 빌려옴.

        obj.transform.SetParent(statusEffectUI.transform); //부모 조정.  
        obj.GetComponent<StatusEffectUI>().SetVariable(effect.duration, effect.duration);
        if (!effectDic.ContainsKey(effect.effectName)) effectDic.Add(effect.effectName, obj); //이 부분은 나중에 수정 예정.
        effectList.Add(obj);
    }

    public void RenewalEffectSlider(StatusEffect effect, string effectName)
    {
        //var obj = effectDic[effectName];

        //effectDic[effectName].gameObject.GetComponent<StatusEffectUI>().SetVariable(effect.duration, effect.duration);
        effectList.Find(n => n == effectDic[effectName]).GetComponent<StatusEffectUI>().SetVariable(effect.duration, effect.duration);
    }

    public void RemoveEffectUI(string effectName)
    {
        effectList.Remove(effectDic[effectName].gameObject);
        //effectDic.Remove(effectName);
    }

    //캔버스에 보이는 상태 이상 관련 UI를 업데이트 하는 함수.
    public void UpdateEffectUI()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(50 + (150 * i), 1);
        }
    }

    public void TextUpdate(string _text)
    {
        //txt.text = _text;
    }

    public void CheckSkillCoolDown(float remainingCoolDown, float coolDown, int skillNum)
    {
        if (remainingCoolDown == coolDown) return;
        skillCoolDown[skillNum].value = remainingCoolDown / coolDown;
    }

    public void CheckDashCoolDown(float remainingCoolDown, float coolDown)
    {
        if (remainingCoolDown == coolDown) return;
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
