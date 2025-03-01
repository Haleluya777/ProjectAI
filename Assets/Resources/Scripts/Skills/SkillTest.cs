using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Skill")]
public class SkillTest : SkillBase
{
    public GameObject fireBallObj;

    public override bool UseSkill()
    {
        if(!base.UseSkill()) return false;

        //스킬의 고유한 기믹 ex) 투사체를 날림, 자가 버프 부여 등.
        Instantiate(fireBallObj, player.transform.position, player.transform.rotation).GetComponent<FireBall>().ObjInit(player.Dir, base.damageCalculator.CalculateDmg(player), attackType.ToString());
        return true;
    }
}
