using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shooting", menuName = "BehaviorTree/Actions/Shooting")]
public class Shooting : EnemyActionSO
{
    [SerializeField] private int attackCount; //공격 횟수
    [SerializeField] private float interval;
    [SerializeField] private GameObject bulletObj; //날려보낼 투사체
    private Transform bulletPos; //투사체가 나타날 위치 

    private int attackTime = 0;

    public override NodeState Execute(EnemyAIController controller)
    {
        Skill_Module skill = controller.LocalBlackboard.Get<Skill_Module>("Skill");
        ISkillCaster caster = controller.LocalBlackboard.Get<ISkillCaster>("SkillCaster");

        if (!controller.LocalBlackboard.HasKey("AttackTime"))
        {
            //공격 실행 후 0.25인터벌 체크.
            //이 곳에 공격 명령어 하나 넣어야 함.
            skill.UseSkill(caster);
            attackTime++;
            controller.LocalBlackboard.Set("AttackTime", Time.time + interval);
            return NodeState.Running;
        }

        else
        {
            if (attackTime < attackCount) //아직 공격을 더 해야 할 때.
            {
                if (Time.time >= controller.LocalBlackboard.Get<float>("AttackTime")) //0.25초가 지난 뒤.
                {
                    //공격!
                    skill.UseSkill(caster);
                    attackTime++;
                    controller.LocalBlackboard.Set("AttackTime", Time.time + interval); //시간 재정의
                    return NodeState.Running;
                }

                else //0.25초가 지나지 않았으면.
                {
                    return NodeState.Running;
                }
            }

            else //AttackCount만큼 공격을 실행했을 때.
            {
                //시퀀스 종료
                attackTime = 0;
                return NodeState.Success;
            }
        }
    }
}
