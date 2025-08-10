using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Passive/TestPassive")]
public class Skill_Passive_Test : SkillBase
{
    RaycastHit2D[] range;
    LineRenderer line;
    public override bool UseSkill(ISkillCaster caster)
    {
        range = Physics2D.CircleCastAll(caster.GetPosition(), 10f, Vector2.zero, 0, 1 << 8);
        line = caster.GetCom<LineRenderer>();

        if (range.Length > 0) // 0이 아닌 경우 (즉, 1개 이상인 경우)
        {
            Debug.Log("범위 내 벽 있음");
            line.positionCount = 2;
            System.Array.Sort(range, (x, y) =>
            {
                if (x.collider == null && y.collider == null) return 0;
                if (x.collider == null) return 1;
                if (y.collider == null) return -1;

                return (x.collider.transform.position - caster.GetPosition()).sqrMagnitude.CompareTo((y.collider.transform.position - caster.GetPosition()).sqrMagnitude);
            });
            line.SetPosition(0, caster.GetPosition());
            line.SetPosition(1, range[0].collider.transform.position);
            Debug.Log("가장 가까운 붙을 수 있는 벽" + range[0].collider.name);
        }
        else
        {
            line.positionCount = 0;
        }

        Debug.Log("패시브 스킬 작동 중.");
        return true;
    }
}
