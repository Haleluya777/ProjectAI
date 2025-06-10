using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Enemy의 상태와 능력치를 관리하는 클래스
// ScriptableObject에서 데이터를 로드하고 관리
public class Enemy_Status_Manager : MonoBehaviour, IDamageable, IInitializable
{
    private Dictionary<string, StatusEffect> activeEffect = new Dictionary<string, StatusEffect>();
    private Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>();
    private Coroutine newCorutine;

    private int currentHp;
    private bool isdead;
    private int phase2Threshold;

    public int CurrentHp => currentHp;
    public int Phase2Threshold => phase2Threshold;
    public bool IsDead => isdead;

    public bool CanAction { get; set; }

    public void DataInitialize(EnemyStatusInfo info, BlackBoard local)
    {
        currentHp = info.Base_Status.HP;

        local.Set("Damaged", this.GetComponent<IDamageable>());
        local.Set("CurrentHp", currentHp);
    }

    public void UpdateDataPerFrame(BlackBoard local)
    {

    }

    public void Damaged(int dmg, string damageType)
    {
        Debug.Log("아프다!");
    }

    private void DamagedProcess(int totalDmg, Color txtColor)
    {
        currentHp -= totalDmg;

        var dmgText = GameManager.instance.objectPoolManger_DmgTxt.Pool.Get();
        dmgText.transform.parent = this.transform.GetChild(0);
        dmgText.transform.localPosition = new Vector2(0, 5.5f);
        dmgText.GetComponent<DmgText>().SetDmgText(totalDmg, txtColor);
    }

    public void StatusEffectProcess(float duration, string effectName)
    {
        ApplyEffect(new Stun(duration, effectName, GetComponent<EnemyController>()));
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
        effect.RemoveEffect();
        activeEffect.Remove(effect.effectName);
        activeEffectCoroutines.Remove(effect.effectName);
    }
}
