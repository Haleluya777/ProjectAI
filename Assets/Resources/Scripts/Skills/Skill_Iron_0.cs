using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

[CreateAssetMenu(menuName = "Skill/Action/Iron/Skill_0")] // 메뉴 경로를 Action으로 명확화
public class Skill_Iron_0 : SkillBase
{
    [SerializeField] private float duration = 0.2f;
    private DOTween currentDG;

    public override bool UseSkill(ISkillCaster caster)
    {
        Transform casterTransform = caster.GetGameObject().transform;
        Vector3 target = parentModule.blackBoard.Get<Vector3>("TargetPos");
        Vector3 dir = (target - caster.GetPosition()).normalized;

        Vector3 newtargetPos = target - (dir * 2.75f);

        caster.SetScale(-1);
        casterTransform.DOMove(newtargetPos, duration).SetEase(Ease.OutQuad).OnComplete(() => { casterTransform.position = newtargetPos; });

        return true;
    }
}
