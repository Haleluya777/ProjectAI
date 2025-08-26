using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryExpand : MonoBehaviour
{
    bool opened = false;
    public GameObject [] elements;
    public void Menuexpand()
    {
        if (!opened)
        {
            foreach (GameObject element in elements)
            {
                element.SetActive(true);
            }
            GetComponent<Animator>().CrossFade("MenuExpand", 0f);
            opened = true;
        }
        else
        {
            foreach (GameObject element in elements)
            {
                element.SetActive(false);
            }
            GetComponent<Animator>().CrossFade("MenuCollapse", 0f);
            opened = false;
        }
    }
}
