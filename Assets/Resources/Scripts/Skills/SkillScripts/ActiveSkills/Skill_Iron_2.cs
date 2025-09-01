using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallingAttack", menuName = "Skill/Action/Iron/FallingAttack")]
public class Skill_Iron_2 : SkillBase
{
    private Rigidbody2D rigid;
    private RaycastHit2D hit;
    Transform casterTransform;
    Vector3 target;
    Vector3 dir;

    public override bool UseSkill(ISkillCaster caster)
    {
        hit = Physics2D.Raycast(caster.GetPosition(), Vector2.down, float.PositiveInfinity, 1 << 6);
        rigid = caster.GetCom<Rigidbody2D>();

        casterTransform = caster.GetGameObject().transform;
        target = hit.point;
        dir = (target - caster.GetPosition()).normalized;

        GameManager.instance.coroutineRunner.StartCoroutine(PerformIronDash(rigid, casterTransform, target));

        return true;
    }

    private IEnumerator PerformIronDash(Rigidbody2D rigid, Transform casterTransform, Vector3 target)
    {
        float dashSpeed = 100f; // 대쉬 속도
        float minSqrDistance = 5f;

        while (((Vector2)target - rigid.position).sqrMagnitude > minSqrDistance)
        {
            Vector2 direction = ((Vector2)target - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }
    }
}
