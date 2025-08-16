using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Passive/TestPassive")]
public class Skill_Passive_Test : SkillBase, IPassiveSkills
{
    RaycastHit2D[] range;
    LineRenderer line;

    RaycastHit2D targetPos;

    public override bool UseSkill(ISkillCaster caster)
    {
        range = Physics2D.CircleCastAll(caster.GetPosition(), 10f, Vector2.zero, 0, 1 << 8);
        line = caster.GetCom<LineRenderer>();

        RaycastHit2D? validTarget = null; // 진짜 유효한 타겟을 담을 변수

        if (range.Length > 0)
        {
            // 1. 거리가 가까운 순으로 정렬합니다.
            System.Array.Sort(range, (x, y) =>
            {
                if (x.collider == null && y.collider == null) return 0;
                if (x.collider == null) return 1;
                if (y.collider == null) return -1;

                return (x.collider.transform.position - caster.GetPosition()).sqrMagnitude.CompareTo((y.collider.transform.position - caster.GetPosition()).sqrMagnitude);
            });

            // 2. 정렬된 배열에서, collider가 null이 아닌 첫 번째 대상을 찾습니다.
            foreach (var hit in range)
            {
                if (hit.collider != null)
                {
                    validTarget = hit; // 찾았으면 validTarget에 저장하고
                    break;             // 루프를 빠져나옵니다.
                }
            }
        }

        // 3. 진짜 유효한 타겟을 찾은 경우에만 로직을 실행합니다.
        if (validTarget.HasValue)
        {
            RaycastHit2D target = validTarget.Value; // 실제 값 가져오기
            line.positionCount = 2;
            line.SetPosition(0, caster.GetPosition());
            line.SetPosition(1, target.collider.transform.position);
            Debug.Log("가장 가까운 붙을 수 있는 벽" + target.collider.name);

            // 블랙보드에 데이터 저장
            if (parentModule != null)
            {
                // Skill_Iron_0 스킬을 위해 GameObject와 위치를 모두 저장해줍니다.
                parentModule.blackBoard.Set("TargetPos", target.collider.transform.position);
            }
        }
        else // 4. 유효한 타겟을 하나도 못 찾은 경우
        {
            line.positionCount = 0;
            // 블랙보드의 데이터도 비워줍니다.
            if (parentModule != null)
            {
                parentModule.blackBoard.Remove("TargetPos");
            }
        }

        return true;
    }

    public void SkillOff()
    {
        line.positionCount = 0;
    }

    public bool Condition()
    {

        return range.Length > 0 ? true : false;
    }
}
