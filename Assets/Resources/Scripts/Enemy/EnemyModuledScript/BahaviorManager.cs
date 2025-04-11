  using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BahaviorManager : MonoBehaviour
{
    //Enemy 의 행동을 관장하는 스크립트.
    //아직 미완.
    
    private BehaviorTreeRunner behaviorTree;

    [SerializeField] private Movement movement;
    [SerializeField] private Combat combat;
    [SerializeField] private Status status;

    private void Awake() 
    {
        ComponentInitialize();
    }

    private void Update() 
    {
        if(behaviorTree != null)
        {
            behaviorTree.Operate();
        }    
    }

    private void ComponentInitialize()
    {
        movement.NodeInitialize();
        combat.NodeInitialize();
    }
}
