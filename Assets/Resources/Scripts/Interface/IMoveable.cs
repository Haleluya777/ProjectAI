using UnityEngine;

public interface IMoveable
{
    bool ShouldMove { get; }
    Transform ParentTransform { get; }
    void MoveToTarget();
    void UpdateDirection(Transform targetPosition);
}
