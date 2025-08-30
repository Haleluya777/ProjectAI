using System.Collections;
using UnityEngine;

public interface IInitializable
{
    void DataInitialize(EnemyStatusInfo info, IBlackBoard local);
    void UpdateDataPerFrame(IBlackBoard local);
}
