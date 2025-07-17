using UnityEngine;

public interface IMoveable
{
    bool ShouldMove { get; }
    Transform ParentTransform { get; }
    void UpdateDirection();
}
