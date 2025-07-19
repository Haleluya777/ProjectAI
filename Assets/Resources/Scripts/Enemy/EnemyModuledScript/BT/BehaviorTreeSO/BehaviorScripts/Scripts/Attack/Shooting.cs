using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shooting", menuName = "BehaviorTree/Actions/Shooting")]
public class Shooting : EnemyActionSO
{
    [SerializeField] private int totalAttacks;
    [SerializeField] private int interval;
    [SerializeField] private int bulletObj; //날려보낼 투사체
    [SerializeField] private Transform bulletPos; //투사체가 나타날 위치 

    private float timer;
    private int attackTime = 0;

    public override NodeState Execute(EnemyAIController controller)
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            if (attackTime < totalAttacks)
            {
                timer = 0;
                attackTime++;
                Debug.Log("공격!");
            }
        }

        if (attackTime >= totalAttacks) return NodeState.Success;

        return NodeState.Running;
    }
}
