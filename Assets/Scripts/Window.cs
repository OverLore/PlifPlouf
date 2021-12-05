using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    [SerializeField] protected bool isOpen = false;

    public virtual void Open()
    {
        if (isOpen)
        {
            return;
        }

        AudioManager.Instance.PlaySound("UIButton");

        animator.SetTrigger("Open");

        isOpen = true;
    }

    public virtual void Close()
    {
        if (!isOpen)
        {
            return;
        }

        AudioManager.Instance.PlaySound("UIButton");

        animator.SetTrigger("Close");

        isOpen = false;
    }
}
