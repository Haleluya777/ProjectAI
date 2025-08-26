using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryText : MonoBehaviour
{
    GameObject textBox;
    public void Showtext()
    {
        textBox = this.gameObject;
        foreach (GameObject story in GameManager.instance.uIManager.story) story.SetActive(false);
        textBox.SetActive(true);
    }
}
