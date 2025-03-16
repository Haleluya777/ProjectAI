using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public void Move(int moveSpeed, GameObject target);
}
