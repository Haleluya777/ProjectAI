using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObjectBase : MonoBehaviour
{
    //투사체 베이스.
    public enum AttackType { Physical, Magic }

    public AttackType attackType;

    public abstract void ObjectMovment();
}
