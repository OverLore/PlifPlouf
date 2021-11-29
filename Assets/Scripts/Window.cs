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

        animator.SetTrigger("Open");

        isOpen = true;
    }

    public virtual void Close()
    {
        if (!isOpen)
        {
            return;
        }

        animator.SetTrigger("Close");

        isOpen = false;
    }
}
