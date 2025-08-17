using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slash", menuName = "Skill/Action/Iron/Slash")]
public class Skill_Slash : SkillBase
{
    Animator anim;
    BoxCollider2D hitBox;
    [SerializeField] private Vector2 hitBoxRange;
    [SerializeField] private Vector2 hitBoxOffSet;

    public override bool UseSkill(ISkillCaster caster)
    {
        Debug.Log("슬래쉬!");
        anim = caster.GetCom<Animator>();
        hitBox = caster.GetHitBox();

        hitBox.size = hitBoxRange;
        hitBox.offset = hitBoxOffSet;
        caster.TotalDmg = damageCalculator.CalculateDmg(caster);
        anim.CrossFade("Slash", 0f);

        return true;
    }
}
