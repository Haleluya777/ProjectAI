using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Enemy_Attack : MonoBehaviour, IAttackable, IInitializable
{
    private List<float> meleeCools;
    private List<float> rangedCools;

    private int att;
    private float mainAttCool;
    private float meleeRange;
    private float rangedRange;
    private int meleeCount;
    private bool canAttack;

    public int Att => att;
    public bool CanAttack => canAttack;
    public float MeleeAttackRange => meleeRange;
    public float RangedAttackRange => rangedRange;
    public int MeleeAttackCount => meleeCount;

    public void DataInitialize(EnemyStatusInfo info, BlackBoard local)
    {
        att = info.Combat_Status.Atk;

        mainAttCool = info.Combat_Status.MainAttCool;
        meleeCools = info.Combat_Status.ShortDisAttCool;
        rangedCools = info.Combat_Status.LongDisAttCool;

        meleeRange = info.Combat_Status.ShortAttackRange;
        rangedRange = info.Combat_Status.LongAttackRange;

        local.Set("Attack", GetComponent<IAttackable>());
        local.Set("AttDamage", att);
        local.Set("MainCool", mainAttCool);
        local.Set("MeleeCools", meleeCools);
        local.Set("RangedCools", rangedCools);
        local.Set("MeleeRange", meleeRange);
        local.Set("RangedRange", rangedRange);
    }

    public void UpdateDataPerFrame(BlackBoard local)
    {
        
    }

    public void GetAvailableAttacks(List<float> cooldowns, List<float> nextTimes, List<int> availableAttacks)
    {

    }

    public bool CheckingCanAttack()
    {
        return true;
    }

    public void PerformAttack()
    {
        //Debug.Log("할렐루야!");
    }
}
