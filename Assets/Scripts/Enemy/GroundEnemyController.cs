using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class GroundEnemyController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public DetectionZone teleportZone;

    Rigidbody2D rb;
    TouchingDirection touchingDirection;
    Animator animator;

    public enum WalkableDirection { Right, Left};

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;

        }

    }

    private bool _hasTarget = false;
    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        set
        {
            _hasTarget = value;
            animator.SetBool("hasTarget", value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool("canMove");
        }
        set
        {
            animator.SetBool("canMove", value);
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
    public float hitProbability = 100f;

    [Header("For teleporting enemies")]
    [SerializeField]
    private float teleportCooldown;
    private bool canTeleport = true;
    [SerializeField]
    private Vector2 teleportOffset;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HasTarget = attackZone.detectedCollider.Count > 0 || teleportZone?.detectedCollider.Count > 0;
        if (teleportZone?.detectedCollider.Count > 0 && canTeleport)
        {
            StartCoroutine(Teleport());
        }
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
        if (touchingDirection.IsOnWall && touchingDirection.IsGrounded || cliffDetectionZone.detectedCollider.Count == 0 && touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
        if (CanMove)
        {
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        } 
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("No walkable direction");
        }
    }

    public void OnHit(float damage, Vector2 knockBack)
    {
        if (!isInvincible)
        {
            if (UnityEngine.Random.value < (hitProbability / 100f))
            {
                animator.SetTrigger("hit");
            }
            isInvincible = true;
            Health -= damage;
            rb.velocity = new Vector2(knockBack.x, knockBack.y);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
        }
    }

    private IEnumerator Teleport()
    {
        canTeleport = false;
        if (gameObject.transform.localScale.x > 0)
        {
            gameObject.transform.position = new Vector2(teleportZone.detectedCollider[0].transform.position.x + -teleportOffset.x, teleportZone.detectedCollider[0].transform.position.y + teleportOffset.y);
            gameObject.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            gameObject.transform.position = new Vector2(teleportZone.detectedCollider[0].transform.position.x + teleportOffset.x, teleportZone.detectedCollider[0].transform.position.y + teleportOffset.y);
            gameObject.transform.localScale = new Vector2(-1, 1);
        }
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }


}
