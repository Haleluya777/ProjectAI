using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BehaviorTreeRunner
{
    private INode _rootNode;
    private BlackBoard _local;
    private BlackBoard _global;
    public BehaviorTreeRunner(INode rootNode, BlackBoard local, BlackBoard global)
    {
        _rootNode = rootNode;
        _local = local;
        _global = global;
    }

    public void Operate()
    {
        //var result = _rootNode.Evaluate(_local, _global);
    }
}
