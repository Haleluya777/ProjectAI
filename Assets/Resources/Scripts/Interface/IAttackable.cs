using UnityEngine;
using System.Collections.Generic;

public interface IAttackable
{
    //float AttackRange { get; }
    float MeleeAttackRange { get; }
    float RangedAttackRange { get; }
    int MeleeAttackCount { get; }

    void GetAvailableAttacks(List<float> cooldowns, List<float> nextTimes, List<int> availableAttacks);
    bool CheckingCanAttack();
    void PerformAttack();
}