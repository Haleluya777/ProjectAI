using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void Damaged(int dmg, string attackType);
    //public void Dead();
}
