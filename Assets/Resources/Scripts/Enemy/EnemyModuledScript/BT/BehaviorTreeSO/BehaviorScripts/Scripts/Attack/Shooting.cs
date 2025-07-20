using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shooting", menuName = "BehaviorTree/Actions/Shooting")]
public class Shooting : EnemyActionSO
{
    [SerializeField] private int totalAttacks;
    [SerializeField] private float interval;
    [SerializeField] private GameObject bulletObj; //날려보낼 투사체
    private Transform bulletPos; //투사체가 나타날 위치 

    private float timer = 0;
    private int attackTime = 0;

    public override NodeState Execute(EnemyAIController controller)
    {
        bulletPos = controller.LocalBlackboard.Get<Transform>("Transform");
        timer += Time.deltaTime;
        
        if (timer >= interval)
        {
            if (attackTime < totalAttacks)
            {
                Debug.Log("공격!");
                timer = 0;
                attackTime++;
                Instantiate(bulletObj, bulletPos);
            }
        }

        if (attackTime >= totalAttacks) return NodeState.Success;

        return NodeState.Running;
    }
}
