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
            System.Array.Sort(range, (x, y) =>
            {
                if (x.collider == null && y.collider == null) return 0;
                if (x.collider == null) return 1;
                if (y.collider == null) return -1;

                return ((Vector3)x.point - caster.GetPosition()).sqrMagnitude.CompareTo(((Vector3)y.point - caster.GetPosition()).sqrMagnitude);
            });

            foreach (var hit in range)
            {
                if (hit.collider != null)
                {
                    validTarget = hit;
                    break;
                }
            }
        }

        if (validTarget.HasValue)
        {
            RaycastHit2D target = validTarget.Value; // 실제 값 가져오기
            line.positionCount = 2;

            line.SetPosition(0, target.point);
            line.SetPosition(1, caster.GetPosition());
            
            Debug.Log("가장 가까운 붙을 수 있는 벽" + target.collider.name);

            if (parentModule != null)
            {
                parentModule.blackBoard.Set("TargetPos", (Vector3)target.point);
                parentModule.blackBoard.Set("Condition", true);
            }
        }
        else
        {
            line.positionCount = 0;
            if (parentModule != null)
            {
                parentModule.blackBoard.Remove("TargetPos");
                parentModule.blackBoard.Set("Condition", false);
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
