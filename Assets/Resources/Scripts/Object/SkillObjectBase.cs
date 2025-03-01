using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObjectBase : MonoBehaviour
{
    public enum AttackType { Physical, Magic }

    public AttackType attackType;

    public abstract void ObjectMovment();
}
