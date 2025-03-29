using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public bool CanAction { get; set; }

    public void Damaged(int dmg, string attackType);
    public void StatusEffectProcess(float duration, string effectName);
    //public void Dead();
}
