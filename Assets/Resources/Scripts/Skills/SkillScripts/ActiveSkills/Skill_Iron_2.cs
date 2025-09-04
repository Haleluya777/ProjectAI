using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallingAttack", menuName = "Skill/Action/Iron/FallingAttack")]
public class Skill_Iron_2 : SkillBase
{
    [SerializeField] private SkillBase chainedSkill; //이 스킬과 연계 되어 사용 될 스킬.

    private Rigidbody2D rigid;
    private RaycastHit2D hit;
    private BoxCollider2D hitBox;
    private int targetLayer;
    Transform casterTransform;
    Vector3 target;
    Vector3 dir;
    Animator anim;

    public override bool UseSkill(ISkillCaster caster)
    {
        hit = Physics2D.Raycast(caster.GetPosition(), Vector2.down, float.PositiveInfinity, 1 << 6 | 1 << 0);
        rigid = caster.GetCom<Rigidbody2D>();
        anim = caster.GetCom<Animator>();
        hitBox = caster.GetHitBox();

        hitBox.size = HitBoxSize;
        hitBox.offset = HitBoxOffSet;
        casterTransform = caster.GetGameObject().transform;
        target = hit.point;
        targetLayer = hit.collider.gameObject.layer;
        dir = (target - caster.GetPosition()).normalized;

        GameManager.instance.coroutineRunner.StartCoroutine(PerformIronDash(rigid, casterTransform, target, targetLayer, caster));

        return true;
    }

    private IEnumerator PerformIronDash(Rigidbody2D rigid, Transform casterTransform, Vector3 target, int targetLayer, ISkillCaster caster)
    {
        float dashSpeed = 100f; // 대쉬 속도
        float minSqrDistance = .5f;

        while (((Vector2)target - rigid.position).magnitude > minSqrDistance)
        {
            Vector2 direction = ((Vector2)target - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }

        if (targetLayer == 0)
        {
            Debug.Log("공중으로 튀어오름");
            rigid.AddForce(Vector2.up * 45, ForceMode2D.Impulse);
            caster.Attacking = false;
        }
        else
        {
            Debug.Log("연결된 스킬 사용");
            chainedSkill.UseSkill(caster);
            caster.Attacking = false;
        }
    }
}
