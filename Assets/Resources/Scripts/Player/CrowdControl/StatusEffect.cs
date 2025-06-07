using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    //모든 상태이상의 기초가 되는 베이스 클래스.
    public string effectName;
    public float duration;
    public IDamageable target;

    public StatusEffect(float duration, IDamageable target)
    {
        this.duration = duration;
        this.target = target;
    }

    public abstract void RemoveEffect();
    public abstract void ApplyEffect();
    public abstract void ResetEffectDuration();
}
