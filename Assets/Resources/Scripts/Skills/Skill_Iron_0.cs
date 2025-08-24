using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Collections;

[CreateAssetMenu(fileName = "IronDash", menuName = "Skill/Action/Iron/IronDash")] // 메뉴 경로를 Action으로 명확화
public class Skill_Iron_0 : SkillBase
{
    [SerializeField] private float duration = 0.2f;
    private DOTween currentDG;
    private WaitForSeconds waitSec;
    private Rigidbody2D rigid;

    Transform casterTransform;
    Vector3 target;
    Vector3 dir;

    public override bool UseSkill(ISkillCaster caster)
    {
        waitSec = new WaitForSeconds(duration);
        rigid = caster.GetCom<Rigidbody2D>();

        casterTransform = caster.GetGameObject().transform;
        target = parentModule.blackBoard.Get<Vector3>("TargetPos");
        dir = (target - caster.GetPosition()).normalized;

        Vector3 newtargetPos = target - (dir * 2f);

        //caster.SetScale(-1);
        //caster.GetCom<Rigidbody2D>().DOMove(newtargetPos, duration).SetEase(Ease.OutQuad).OnComplete(() => { casterTransform.position = newtargetPos; });

        GameManager.instance.coroutineRunner.StartRunnerCoroutine(PerformIronDash(dir, rigid, casterTransform, target));

        return true;
    }

    private IEnumerator PerformIronDash(Vector3 dir, Rigidbody2D rigid, Transform casterTransform, Vector3 target)
    {
        //if (casterTransform.position != target)
        //{
        //    Vector3 newPos = (Vector3)rigid.position + target * 5f * Time.deltaTime;
        //    rigid.MovePosition(newPos);
        //}
        //else
        //{
        //    //rigid.velocity = Vector2.zero;
        //    yield return null;
        //}
        float dashSpeed = 20f; // 대쉬 속도
        float minSqrDistance = 2f; // 목표 지점 근접 오차 (거리의 제곱)

        // 목표 지점과 충분히 멀리 떨어져 있을 때까지 반복
        while ((rigid.position - (Vector2)target).sqrMagnitude > minSqrDistance)
        {
            Debug.Log("아직 도착 안함.");
            // 다음 위치를 계산 (Time.fixedDeltaTime 사용)
            Vector3 newPos = (Vector3)rigid.position + target * dashSpeed * Time.deltaTime;

            // Rigidbody를 물리적으로 이동
            rigid.MovePosition(newPos);

            // 다음 물리 프레임까지 대기
            yield return new WaitForFixedUpdate();
        }
    }
}
