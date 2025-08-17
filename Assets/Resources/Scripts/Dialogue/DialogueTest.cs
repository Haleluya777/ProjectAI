using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private Text txt;

    [SerializeField] private List<string> dialogue = new List<string>();
    int i = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && i < dialogue.Count)
        {
            Debug.Log(dialogue[i]);
            txt.text = dialogue[i];
            i++;
        }
    }
}
