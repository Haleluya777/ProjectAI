using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, ITriggerable
{
    private BlackBoard local = new BlackBoard();
    IDamageable damaged;
    [SerializeField] private int dmg;

    private void OnEnable()
    {
        local.Set("Trigger", true);
    }

    private void OnDisable()
    {
        local.Clear();
    }

    public BlackBoard GetBlackBoard()
    {
        return local;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (local.Get<bool>("Trigger"))
        {
            damaged = other.GetComponent<IDamageable>();
            if (other.gameObject.tag == "Player" && damaged != null)
            {
                Debug.Log("Get Damaged!");
                damaged.Damaged(dmg, "Physical");
            }
        }
    }
}
