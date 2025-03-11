using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public float duration;
    public PlayerController target;

    public StatusEffect(float duration, PlayerController target)
    {
        this.duration = duration;
        this.target = target;
    }

    public abstract void RemoveEffect();
    public abstract void ApplyEffect();
}
