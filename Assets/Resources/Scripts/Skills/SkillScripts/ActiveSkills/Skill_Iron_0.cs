using UnityEngine;
using DG.Tweening;
using System.Collections;

[CreateAssetMenu(fileName = "IronDash", menuName = "Skill/Action/Iron/IronDash")] // 메뉴 경로를 Action으로 명확화
public class Skill_Iron_0 : SkillBase
{
    private WaitForSeconds waitSec;
    private Rigidbody2D rigid;

    Transform casterTransform;
    Vector3 target;

    public override bool UseSkill(ISkillCaster caster)
    {
        rigid = caster.GetCom<Rigidbody2D>();

        casterTransform = caster.GetGameObject().transform;
        target = parentModule.blackBoard.Get<Vector3>("TargetPos");

        //caster.SetScale(-1);
        //caster.GetCom<Rigidbody2D>().DOMove(newtargetPos, duration).SetEase(Ease.OutQuad).OnComplete(() => { casterTransform.position = newtargetPos; });

        GameManager.instance.coroutineRunner.StartRunnerCoroutine(PerformIronDash(rigid, casterTransform, target));

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
