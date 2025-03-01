using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class DmgText : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    public TextMeshProUGUI txt;
    
    private const int MOVE_SPEED = 3;

    private void Awake() 
    {
        txt = this.GetComponent<TextMeshProUGUI>();
    }

    public void SetDmgText(int dmg, Color color)
    {
        txt.color = color;
        txt.text = dmg.ToString();
    }

    private void Update() 
    {
        this.transform.Translate(Vector2.up * MOVE_SPEED * Time.deltaTime);    
    }

    public void ReleaseThisObj()
    {
        Pool.Release(this.gameObject);
    }
}
