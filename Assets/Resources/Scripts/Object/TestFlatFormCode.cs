using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestFlatFormCode : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Up");
            this.transform.DOMove(new Vector2(this.transform.position.x, this.transform.position.y + 10), .5f);
        }    
    }
}
