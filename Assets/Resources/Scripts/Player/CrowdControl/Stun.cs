using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    public Stun(float duration, string effectName, IDamagable target) : base(duration, target)
    {
        base.effectName = effectName;
    }

    public override void ApplyEffect()
    {
        this.target.CanAction = false;
        Debug.Log("스턴!");
    }

    public override void RemoveEffect()
    {
        target.CanAction = true;
    }

    public override void ResetEffectDuration()
    {
        
    }
}
