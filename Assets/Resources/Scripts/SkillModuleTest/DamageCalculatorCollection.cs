using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "DamageCalculator/Collection")]
public class DamageCalculatorCollection : ScriptableObject
{
    public DamageCalculator[] damageCalculators;
}
