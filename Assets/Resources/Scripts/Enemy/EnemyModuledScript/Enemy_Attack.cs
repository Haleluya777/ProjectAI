using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;

public class Enemy_Attack : MonoBehaviour, IAttackable, IInitializable, IRequiredAnimator, ISkillCaster
{
    private Animator anim;
    private IBlackBoard blackBoard;
    private List<float> meleeCools;
    private List<float> rangedCools;
    
    //투사체를 쏘는 원거리 공격을 하는 경우에만 사용.
    [SerializeField] private Transform shootingPos; 
    [SerializeField] private GameObject shootingObj;
    //

    //모듈화 된 스킬
    [SerializeField] private Skill_Module skill;
    //

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
        blackBoard.Set("SkillCaster", GetComponent<ISkillCaster>());
        blackBoard.Set("AttDamage", att);
        blackBoard.Set("MainCool", mainAttCool);
        blackBoard.Set("MeleeCools", meleeCools);
        blackBoard.Set("RangedCools", rangedCools);
        blackBoard.Set("ShootingPoint", shootingPos);

        blackBoard.Set("DetectionRange", detectionRange);
        blackBoard.Set("EscapeRange", escapeRange);
        blackBoard.Set("GuardRange", guardRange);
        blackBoard.Set("MaxGuardGage", 3f);

        blackBoard.Set("MeleeRange", meleeRange);
        blackBoard.Set("RangedRange", rangedRange);

        blackBoard.Set("Guarding", false);
        blackBoard.Set("CanAttack", false);
        blackBoard.Set("Attacking", false);

        blackBoard.Set("Skill", skill);
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

    public Vector3 GetPosition()
    {
        return blackBoard.Get<Transform>("ShootingPos").position;
    }

    public Quaternion GetRotation()
    {
        return blackBoard.Get<Transform>("ShootingPos").rotation;
    }

    public int GetAttackPower()
    {
        return att;
    }

    public IDamageable GetDamageableComponent()
    {
        return this.gameObject.transform.parent.GetComponentInChildren<IDamageable>();
    }

    public GameObject GetGameObject()
    {
        return shootingObj;
    }
}
