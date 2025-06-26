using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Dash")]
public class Skill_Dash : SkillBase
{
    //스킬이 가지는 고유값.
    [SerializeField] private int dashDistance; //대쉬 거리
    [SerializeField] private int duration; //목표 도달까지 걸리는 시간
    public override bool UseSkill()
    {
        if (!base.UseSkill()) return false;
        Debug.Log("대쉬 스킬!");
        //대쉬 주요 기믹
        //StartCoroutine(MoveToTarget(new Vector3(0, 0, 0), 0.5f));
        //
        return true;
    }

    IEnumerator MoveToTarget(Vector3 target, float duration)
    {
        float timeElapsed = 0f; // 경과 시간
        Vector3 startPos = caster.GetGameObject().transform.position; // 코루틴 시작 시점의 위치

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            caster.GetGameObject().transform.position = Vector3.Lerp(startPos, target, t);

            timeElapsed += Time.deltaTime; // 다음 프레임까지의 시간 증가
            yield return null; // 다음 프레임까지 대기
        }
        caster.GetGameObject().transform.position = target;
        Debug.Log("이동 완료!");
    }
}
