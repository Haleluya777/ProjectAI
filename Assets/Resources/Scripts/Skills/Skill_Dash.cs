using UnityEngine;
using DG.Tweening; // DOTween 라이브러리 사용

[CreateAssetMenu(menuName = "Skill/Action/Dash")] // 메뉴 경로를 Action으로 명확화
public class Skill_Dash : SkillBase
{
    [SerializeField] private float dashDistance = 5f; // 대쉬 거리
    [SerializeField] private float duration = 0.2f;   // 대쉬 시간

    public override bool UseSkill(ISkillCaster caster)
    {
        Transform casterTransform = caster.GetGameObject().transform;

        // 캐릭터가 바라보는 방향으로 대쉬
        float direction = Mathf.Sign(casterTransform.localScale.x);
        float targetX = casterTransform.position.x + (direction * dashDistance);

        // DOTween을 사용하여 부드러운 이동 구현
        casterTransform.DOMoveX(targetX, duration).SetEase(Ease.OutQuad);

        Debug.Log($"{caster.GetGameObject().name}이(가) 대쉬 사용!");
        return true;
    }
}