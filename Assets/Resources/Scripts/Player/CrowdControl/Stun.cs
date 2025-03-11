using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    public Stun(float duration, PlayerController target) : base(duration, target)
    {
        
    }

    public override void ApplyEffect()
    {
        this.target.canAction = false;
        Debug.Log("스턴!");
    }

    public override void RemoveEffect()
    {
        target.canAction = true;
    }
}
