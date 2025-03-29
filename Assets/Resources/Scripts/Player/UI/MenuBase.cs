using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuBase : MonoBehaviour
{
    public virtual void MenuActivate()
    {
        Debug.Log("메뉴 실행");
    }
}
