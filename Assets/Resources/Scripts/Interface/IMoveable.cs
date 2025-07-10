using UnityEngine;

public interface IMoveable
{
    bool ShouldMove { get; }
    Transform ParentTransform { get; }
    void MoveToTarget(Transform target);
    void UpdateDirection(Transform targetPosition);
}
