using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/TestSkill")]
public class Skill_TestSkill : SkillBase
{
    public override bool UseSkill()
    {
         if(!base.UseSkill()) return false;
        Debug.Log("할렐루야!");
         return true;
    }
}
