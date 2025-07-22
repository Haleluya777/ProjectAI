using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;

public class Enemy_Attack : MonoBehaviour, IAttackable, IInitializable, IRequiredAnimator
{
    private Animator anim;
    private IBlackBoard blackBoard;
    private List<float> meleeCools;
    private List<float> rangedCools;

    private int att;
    private float mainAttCool;
    private float detectionRange;
    private float guardRange;
    private float escapeRange;
    private float meleeRange;
    private float rangedRange;
    private int meleeCount;
    private bool canAttack;

    public int Att => att;
    public bool CanAttack => canAttack;
    public float MeleeAttackRange => meleeRange;
    public float RangedAttackRange => rangedRange;
    public int MeleeAttackCount => meleeCount;

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local)
    {
        blackBoard = local;

        att = info.Combat_Status.Atk;

        mainAttCool = info.Combat_Status.MainAttCool;
        meleeCools = info.Combat_Status.ShortDisAttCool;
        rangedCools = info.Combat_Status.LongDisAttCool;

        guardRange = info.Combat_Status.GuardRange;
        detectionRange = info.Combat_Status.DetectionRange;
        escapeRange = info.Combat_Status.EscapeRange;

        meleeRange = info.Combat_Status.ShortAttackRange;
        rangedRange = info.Combat_Status.LongAttackRange;

        blackBoard.Set("Attack", GetComponent<IAttackable>());
        blackBoard.Set("AttDamage", att);
        blackBoard.Set("MainCool", mainAttCool);
        blackBoard.Set("MeleeCools", meleeCools);
        blackBoard.Set("RangedCools", rangedCools);

        blackBoard.Set("DetectionRange", detectionRange);
        blackBoard.Set("EscapeRange", escapeRange);
        blackBoard.Set("GuardRange", guardRange);

        blackBoard.Set("MeleeRange", meleeRange);
        blackBoard.Set("RangedRange", rangedRange);

        blackBoard.Set("Guarding", false);
        blackBoard.Set("CanAttack", false);
        blackBoard.Set("Attacking", false);
    }

    public void UpdateDataPerFrame(IBlackBoard local)
    {
        blackBoard.Set("MainCoolRegain", blackBoard.Get<float>("MainCoolRegain") + Time.deltaTime);
    }

    public void InjectAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void GetAvailableAttacks(List<float> cooldowns, List<float> nextTimes, List<int> availableAttacks)
    {

    }

    public bool CheckingCanAttack()
    {
        return true;
    }

    public void Aelrting()
    {
        blackBoard.Set("CanAttack", true);
    }

    public void PerformAttack()
    {
        anim.CrossFade("Enemy_Attack", 0f);
    }
}
