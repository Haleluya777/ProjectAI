using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpRise", menuName = "Skill/Action/Fire/UpRise")]

public class Skill_Fire_UpRise : SkillBase
{
    Vector3 targetPos;
    Rigidbody2D rigid;
    Transform casterTransform;
    BoxCollider2D hitBox;
    public override bool UseSkill(ISkillCaster caster)
    {
        rigid = caster.GetCom<Rigidbody2D>();
        casterTransform = caster.GetGameObject().transform;
        targetPos = new Vector3(casterTransform.position.x + (caster.GetGameObject().transform.localScale.x * 1), casterTransform.position.y + 10, 0);
        hitBox = caster.GetHitBox();

        hitBox.size = HitBoxSize;
        hitBox.offset = HitBoxOffSet;

        GameManager.instance.coroutineRunner.StartCoroutine(PerformUpRise(rigid, casterTransform, targetPos));
        return true;
    }

    private IEnumerator PerformUpRise(Rigidbody2D rigid, Transform casterTransform, Vector3 target)
    {
        float dashSpeed = 50f; // 대쉬 속도
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
