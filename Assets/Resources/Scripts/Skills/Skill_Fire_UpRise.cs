using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpRise", menuName = "Skill/Action/Fire/UpRise")]
public class Skill_Fire_UpRise : SkillBase
{
    public override bool UseSkill(ISkillCaster caster)
    {

        return true;
    }

    private IEnumerator PerformUpRise(Rigidbody2D rigid, Transform casterTransform, Vector3 target)
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
