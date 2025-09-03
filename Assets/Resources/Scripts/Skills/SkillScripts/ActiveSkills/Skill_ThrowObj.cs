using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Action/ThrowObj")] // 메뉴 경로를 Action으로 명확화
public class Skill_ThrowObj : SkillBase
{
    public GameObject throwableObj;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (throwableObj == null)
        {
            Debug.LogError("투사체 오브젝트가 할당되어 있지 않음.");
            return false;
        }

        // Caster의 위치와 방향에 프리팹 생성
        GameObject objInstance = Instantiate(throwableObj, caster.GetPosition(), caster.GetRotation());

        // FireBall 스크립트에 데미지와 사용자 정보 전달
        SkillObjectBase objComponent = objInstance.GetComponent<SkillObjectBase>();
        if (objComponent != null)
        {
            // damageCalculator를 사용하여 데미지 계산
            int calculatedDamage = 0;
            if (damageCalculator != null)
            {
                calculatedDamage = damageCalculator.CalculateDmg(caster);
            }
            else
            {
                //Debug.LogWarning("Damage Calculator가 할당되지 않아 기본 데미지 0으로 설정됩니다.");
            }
            //임시 값 할당.
            objComponent.ObjInit(caster.GetDirection(), calculatedDamage, damagType.ToString(), caster.GetTag());
        }
        else
        {
            //Debug.LogError("FireBall Prefab에 FireBall 컴포넌트가 없음.");
        }

        //Debug.Log("파이어볼 발사!");
        caster.Attacking = false;
        return true;
    }
}
