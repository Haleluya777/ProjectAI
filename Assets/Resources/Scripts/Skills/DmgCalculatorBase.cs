using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DmgCalculatorBase : ScriptableObject
{
    public abstract int CalculateDmg(PlayerController attacker);
}
