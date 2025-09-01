using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameWave", menuName = "Skill/Action/Fire/FlameWave")]
public class Skill_Fire_FlameWave : SkillBase
{
    public GameObject flameObj;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (flameObj != null)
        {
            Debug.Log("투사체 오브젝트 할당 되지 않음.");
            return false;
        }

        

        return true;
    }
}
