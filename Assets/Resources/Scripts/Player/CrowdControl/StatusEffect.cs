using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public string effectName;
    public float duration;
    public IDamagable target;

    public StatusEffect(float duration, IDamagable target)
    {
        this.duration = duration;
        this.target = target;
    }

    public abstract void RemoveEffect();
    public abstract void ApplyEffect();
    public abstract void ResetEffectDuration();
}
