using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(menuName = "Skill/Dash")]
public class Skill_Dash : SkillBase
{
    //스킬이 가지는 고유값.
    [SerializeField] private int dashDistance; //대쉬 거리
    [SerializeField] private float duration; //목표 도달까지 걸리는 시간
    private int dir;
    
    public override bool UseSkill()
    {
        if (!base.UseSkill()) return false;

        Debug.Log("대쉬 스킬!");
        //대쉬 주요 기믹
        dir = Caster.GetGameObject().transform.localScale.x == 1 ? 1 : -1;
        Caster.GetGameObject().transform.DOMoveX(Caster.GetPosition().x + (dir * dashDistance), duration);
        //
        return true;
    }
}
