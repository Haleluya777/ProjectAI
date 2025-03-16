  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BahaviorManager : MonoBehaviour
{
    private BehaviorTreeRunner behaviorTree;

    private void Awake() 
    {
        BehaviorTreeInitialize();
    }

    private void Update() 
    {
        if(behaviorTree != null)
        {
            behaviorTree.Operate();
        }    
    }

    private void BehaviorTreeInitialize()
    {

    }
}
