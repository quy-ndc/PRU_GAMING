using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopController : MonoBehaviour
{
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

    public void OnTalkedTo(InputAction.CallbackContext context)
    {
        if (context.started && isInRange)
        {
            CharacterEvents.characterTalk.Invoke(gameObject, new Vector2(0.5f, 0.5f), "Hi there");
        }
    }
}
