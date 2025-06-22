using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Dash")]
public class Skill_Dash : SkillBase
{
    //스킬이 가지는 고유값.
    [SerializeField] private int dashDistance; //대쉬 거리
    [SerializeField] private int dashSpeed; //대쉬 속도
    public override bool UseSkill()
    {
        if (!base.UseSkill()) return false;
        Debug.Log("대쉬 스킬!");
        //대쉬 주요 기믹
        //caster.GetGameObject().transform.Translate(new Vector3(0,0,0));
        caster.GetGameObject().transform.position = new Vector3(0, 0, 0);
        //
        return true;
    }
}
