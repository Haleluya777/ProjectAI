using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Enemy의 상태와 능력치를 관리하는 클래스
// ScriptableObject에서 데이터를 로드하고 관리
public class Enemy_Status_Manager : MonoBehaviour, IDamageable, IInitializable, IRequiredAnimator
{
    private Dictionary<string, StatusEffect> activeEffect = new Dictionary<string, StatusEffect>();
    private Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>();
    private Coroutine newCorutine;
    private Animator anim;
    private int maxHp;
    private int currentHp;
    private int defense;
    private int magicalDefense;
    private bool isdead;
    private int phase2Threshold;

    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    public int Phase2Threshold => phase2Threshold;
    public bool IsDead => isdead;

    public bool CanAction { get; set; }

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local)
    {
        CanAction = true;
        maxHp = info.Base_Status.HP;
        currentHp = maxHp;
        defense = info.Base_Status.Defense;
        magicalDefense = info.Base_Status.MagicalDefense;
    }

    public void UpdateDataPerFrame(IBlackBoard local)
    {
        local.Set("CanAction", CanAction);
    }

    public void InjectAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void Damaged(int dmg, string damageType)
    {
        Color txtcolor = new Color();
        int totalDmg = new int();

        if (damageType == "Physical")
        {
            totalDmg = dmg - defense;
            txtcolor = Color.white;
        }

        else if (damageType == "Magical")
        {
            totalDmg = dmg - magicalDefense;
            txtcolor = Color.blue;
        }

        DamagedProcess(totalDmg, txtcolor);
    }

    private void DamagedProcess(int totalDmg, Color txtColor)
    {
        currentHp -= totalDmg;

        var dmgText = GameManager.instance.objectPoolManger_DmgTxt.Pool.Get();
        dmgText.transform.parent = this.transform.parent.transform.GetChild(0);
        dmgText.transform.localPosition = new Vector2(0, 5.5f);
        dmgText.GetComponent<DmgText>().SetDmgText(totalDmg, txtColor);
    }

    public void StatusEffectProcess(float duration, string effectName)
    {
        ApplyEffect(new Stun(duration, effectName, GetComponent<Enemy_Status_Manager>()));
    }

    private void ApplyEffect(StatusEffect effect) //상태 이상 적용.
    {
        if (!activeEffect.ContainsKey(effect.effectName)) //적용하려는 상태 이상이 현재 플레이어에게 작용하고 있지 않을 경우.
        {
            activeEffect.Add(effect.effectName, effect);
            effect.ApplyEffect();
            newCorutine = StartCoroutine(RemoveEffectAfterDuration(effect));
            activeEffectCoroutines.Add(effect.effectName, newCorutine);
        }

        else //적용하려는 상태 이상이 현재 플레이어에게 작용하고 있는 경우.
        {
            if (activeEffectCoroutines.TryGetValue(effect.effectName, out Coroutine runningCoroutine))
            {
                StopCoroutine(runningCoroutine);
                activeEffectCoroutines[effect.effectName] = StartCoroutine(RemoveEffectAfterDuration(effect));
            }
        }
    }

    public void Dead()
    {
        Debug.Log("죽었다!");
    }
    
    IEnumerator RemoveEffectAfterDuration(StatusEffect effect) //상태 이상 제거.
    {
        yield return new WaitForSeconds(effect.duration);
        Debug.Log("상태이상 사라짐!");
        effect.RemoveEffect();
        activeEffect.Remove(effect.effectName);
        activeEffectCoroutines.Remove(effect.effectName);
    }
}
