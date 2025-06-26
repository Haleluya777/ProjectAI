using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/FireBall")]
public class Skill_FireBall : SkillBase
{
    public GameObject fireBallObj;

    public override bool UseSkill()
    {
        if (!base.UseSkill()) return false;

        //Debug.Log(player == null);
        //스킬의 고유한 기믹 ex) 투사체를 날림, 자가 버프 부여 등.
        
        GameObject fireball = Instantiate(fireBallObj, Caster.GetPosition(), Caster.GetRotation());
        //투사체의 방향을 결정할 변수
        fireball.GetComponent<FireBall>().ObjInit(Caster.GetGameObject().transform, base.damageCalculator.CalculateDmg(Caster), attackType.ToString());
        return true;
    }
}
