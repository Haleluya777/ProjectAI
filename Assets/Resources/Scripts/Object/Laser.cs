using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    IDamageable damaged;
    [SerializeField] private int dmg;

    private void OnTriggerEnter2D(Collider2D other)
    {
        damaged = other.GetComponent<IDamageable>();
        if (other.gameObject.tag == "Player" && damaged != null)
        {
            Debug.Log("Get Damaged!");
            damaged.Damaged(dmg, "Physical");
        }
    }
}
