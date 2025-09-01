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
    private enum AttackType { Melee, Range }
    private Animator anim;
    private IBlackBoard blackBoard;
    private List<float> meleeCools;
    private List<float> rangedCools;
    [SerializeField] private BoxCollider2D hitBox;

    //투사체를 쏘는 원거리 공격을 하는 경우에만 사용.
    [SerializeField] private Transform shootingPos;
    //

    //모듈화 된 스킬
    [SerializeField] private Skill_Module skillTemplate;
    private Skill_Module skill;
    //

    //공격 타입 (원,근거리)
    [SerializeField] private AttackType attackType;
    //

    private int att;
    private int totalDmg;
    private float mainAttCool;
    private float detectionRange;
    private float guardRange;
    private float escapeRange;
    private float meleeRange;
    private float rangedRange;
    private int meleeCount;
    private bool canAttack;

    public int Att => att;
    public int TotalDmg { get; set; }
    public bool CanAttack => canAttack;
    public float MeleeAttackRange => meleeRange;
    public float RangedAttackRange => rangedRange;
    public int MeleeAttackCount => meleeCount;

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local)
    {
        blackBoard = local;
        if (skillTemplate != null) skill = Instantiate(skillTemplate);

        att = info.Combat_Status.Atk;

        mainAttCool = info.Combat_Status.MainAttCool;
        meleeCools = info.Combat_Status.ShortDisAttCool;
        rangedCools = info.Combat_Status.LongDisAttCool;

        guardRange = info.Combat_Status.GuardRange;
        detectionRange = info.Combat_Status.DetectionRange;
        escapeRange = info.Combat_Status.EscapeRange;

        meleeRange = info.Combat_Status.ShortAttackRange;
        rangedRange = info.Combat_Status.LongAttackRange;

        blackBoard.Set("Attack", this.GetComponent<IAttackable>());
        blackBoard.Set("SkillCaster", this.GetComponent<ISkillCaster>());

        blackBoard.Set("AttackType", attackType);

        blackBoard.Set("AttDamage", att);
        blackBoard.Set("MainCool", mainAttCool);
        blackBoard.Set("MeleeCools", meleeCools);
        blackBoard.Set("RangedCools", rangedCools);
        blackBoard.Set("ShootingPos", shootingPos);

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
        if (skill != null)
        {
            skill.UpdateCoolDown(Time.deltaTime);
        }
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

    public void SetScale(int dir)
    {

    }

    public BoxCollider2D GetHitBox()
    {
        return hitBox;
    }

    public Vector3 GetPosition()
    {
        return blackBoard.Get<Transform>("ShootingPos").position;
    }

    public Vector3 GetDirection()
    {
        return blackBoard.Get<Vector3>("AimDirection");
    }

    public Quaternion GetRotation()
    {
        return blackBoard.Get<Transform>("ShootingPos").rotation;
    }

    public int GetAttackPower()
    {
        return att;
    }

    public string GetTag()
    {
        return this.transform.parent.tag;
    }

    public IDamageable GetDamageableComponent()
    {
        return this.gameObject.transform.parent.GetComponentInChildren<IDamageable>();
    }

    public GameObject GetGameObject()
    {
        return this.transform.parent.gameObject;
    }

    public T GetCom<T>() => this.GetCom<T>();
}
