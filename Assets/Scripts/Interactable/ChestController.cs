using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestController : MonoBehaviour
{
    public bool IsOpen
    {
        get
        {
            return animator.GetBool("isOpen");
        }
        set
        {
            animator.SetBool("isOpen", value);
        }
    }

    public bool isInRange = false;

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInRange = false;
    }

    public void OnOpen (InputAction.CallbackContext context)
    {
        if(context.started && isInRange && !IsOpen)
        {
            IsOpen = true;
            GameManager.Instance.OnChestOpened();
        }
    }
}
