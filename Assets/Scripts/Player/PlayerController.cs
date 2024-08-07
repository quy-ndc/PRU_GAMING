using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    public float dashingPower = 5;
    public float dashingTime = 1/2;
    public float dashingCooldown = 1/2;
    private bool canDash = true;
    private bool isDashing;

    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 moveInput;
    public TouchingDirection touchingDirection;
	public HeathBar healthBar;

	private bool _isMoving = false;
    private bool _isFacingRight = true;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirection.IsOnWall && !IsBlocking)
                {
                    if (!IsSlowed)
                    {
                        return moveSpeed;
                    }
                    else
                    {
                        return moveSpeed / 2;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
         set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    [SerializeField]
    private bool _isBlocking;
    public bool IsBlocking
    {
        get
        {
            return _isBlocking;
        }
        set
        {
            _isBlocking = value;
            animator.SetBool("isBlocking", value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool("canMove");
        }
    }

    public bool IsSlowed
    {
        get
        {
            return animator.GetBool("isSlowed");
        }
    }

    [SerializeField]
    private float _maxHealth;
    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private float _health;
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                IsAlive = false;
                GameManager.Instance.RestartGame();
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool("isAlive", value);
        }
    }

    [SerializeField]
    private bool isInvincible = false;
    private float timeSinceHit;
    public float invincibilityTime = 0.25f;


    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        healthBar.gameObject.SetActive(true);
		healthBar.UpdateBar(Health, MaxHealth);
	}

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded && CanMove)
        {
            animator.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("attack");
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded)
        {
            IsBlocking = true;
        }
        if (context.canceled)
        {
            IsBlocking = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash && CanMove)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animator.SetTrigger("dash");
        rb.gravityScale = 0f;
        float dashDirection = IsFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashDirection * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime - 1/2);
        rb.gravityScale = 3;
        yield return new WaitForSeconds(1/2);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void OnHit(float damage, Vector2 knockBack)
    {
        if (!IsBlocking && !isInvincible)
        {
            animator.SetTrigger("hit");
            isInvincible = true;
            Health -= damage;
			healthBar.UpdateBar(Health, MaxHealth);
			rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
        }
    }

    public void OnBlocked(float damage, Vector2 knockBack)
    {
        isInvincible = true;
        Health -= damage / 2;
		healthBar.UpdateBar(Health, MaxHealth);
		CharacterEvents.characterBlocked.Invoke(gameObject, damage / 2);
    }
}
