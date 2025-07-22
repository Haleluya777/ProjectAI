using UnityEngine;

public interface IMovable
{
    bool ShouldMove { get; }
    Transform ParentTransform { get; }
    void UpdateDirection();
    void MoveToTarget();
}
