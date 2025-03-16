  using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BahaviorManager : MonoBehaviour
{
    //Enemy 의 행동을 관장하는 스크립트.
    private BehaviorTreeRunner behaviorTree;

    [SerializeField] private Movement movement;
    [SerializeField] private Attack attack;
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
        attack.NodeInitialize();
    }
}
