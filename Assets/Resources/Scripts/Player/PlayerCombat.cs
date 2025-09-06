using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckGetDamage(other); //데미지를 주는 메서드.
    }

    private void CheckGetDamage(Collider2D other)
    {
        //if (other.GetComponentInChildren<IDamageable>() != null && attacking)
        //{
        //    IDamageable damagable = other.GetComponentInChildren<IDamageable>();
        //    damagable.Damaged(TotalDmg, "Physical");
        //    //damagable.StatusEffectProcess(5f, "Stun");
        //    GameManager.instance.InBattleState();
        //}
        Debug.Log("맞혔다!");
    }
}
