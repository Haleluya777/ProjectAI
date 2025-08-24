using UnityEngine;
using DG.Tweening;
using System.Collections;

[CreateAssetMenu(menuName = "Skill/Action/Dash")]
public class Skill_Dash : SkillBase
{
    [SerializeField] private float dashDistance = 5f; // 대쉬 거리
    [SerializeField] private float duration = 0.2f;   // 대쉬 시간

    public override bool UseSkill(ISkillCaster caster)
    {
        Transform casterTransform = caster.GetGameObject().transform;

        float direction = Mathf.Sign(casterTransform.localScale.x);
        float targetX = casterTransform.position.x + (direction * dashDistance);

        Vector3 target = new Vector3(targetX, caster.GetGameObject().transform.position.y, caster.GetGameObject().transform.position.z);

        GameManager.instance.coroutineRunner.StartRunnerCoroutine(PerformDash(caster, caster.GetCom<Rigidbody2D>(), casterTransform, target));

        return true;
    }

    private IEnumerator PerformDash(ISkillCaster caster, Rigidbody2D rigid, Transform casterTransform, Vector3 target)
    {
        float dashSpeed = 100f; // 대쉬 속도
        float minSqrDistance = 5f;

        while (((Vector2)target - rigid.position).sqrMagnitude > minSqrDistance)
        {
            Debug.Log("대쉬중");
            if (Physics2D.Raycast(caster.GetPosition(), Vector2.right * caster.GetGameObject().transform.localScale.x, 2f, 1 << 6))
            {
                Debug.Log("대쉬 취소");
                break;
            }

            Vector2 direction = ((Vector2)target - rigid.position).normalized;
            Vector3 newPos = rigid.position + direction * dashSpeed * Time.fixedDeltaTime;

            rigid.MovePosition(newPos);

            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}