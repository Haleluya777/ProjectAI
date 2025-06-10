using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface IInitializable
{
    void DataInitialize(EnemyStatusInfo info, BlackBoard local);
    void UpdateDataPerFrame(BlackBoard local);
}
