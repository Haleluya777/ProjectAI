using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHash : MonoBehaviour
{
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
        int hash1 = Animator.StringToHash("Light");
        int hash2 = Animator.StringToHash("Metal");
        int hash3 = Animator.StringToHash("Fire");
        int hash4 = Animator.StringToHash("Water");
        int hash5 = Animator.StringToHash("Tree");
        int hash6 = Animator.StringToHash("Soil");
        int hash7 = Animator.StringToHash("Darkness");
        int hash8 = Animator.StringToHash("Skill1");
        int hash9 = Animator.StringToHash("Skill2");
        int hash10 = Animator.StringToHash("Skill3");
        int hash11 = Animator.StringToHash("Skill4");
        Debug.Log(hash1 +" "+hash2 +" "+ hash3 +" "+ hash4 +" "+ hash5 +" "+ hash6 +" "+ hash7 +" "+ hash8 +" "+ hash9 +" "+ hash10 +" "+ hash11);
    }
}
