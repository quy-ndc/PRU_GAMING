﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Boss : MonoBehaviour
{
	private Animator animator;
	private Transform player;
	public float speed;
	private Rigidbody2D rb2d;
	public bool isFlipped = false;
	[SerializeField]
	public GameObject healthBar;
	[SerializeField]
	public DetectionZone attackZone;
	public float health = 500;
	[SerializeField]
	private bool isInvincible = false;
	private float timeSinceHit;
	public float invincibilityTime = 0.25f;
	public float hitProbability = 100f;

	private bool _isActive;
	public bool IsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			animator.SetBool("isActive", value);
			_isActive = value;
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

	public static Boss Instance;

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
	}

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		animator = GetComponent<Animator>();
		Collider2D[] colliders = GetComponents<Collider2D>();
		rb2d = animator.GetComponent<Rigidbody2D>();
		foreach (Collider2D collider in colliders)
		{
			collider.enabled = true;
		}

		if (player != null)
		{
			Debug.Log("Player found and target assigned.");
		}
		else
		{
			Debug.LogError("Player not found! Make sure the player GameObject has the tag 'Player'.");
		}
	}
	public void MoveTowardsPlayer(float moveSpeed)
	{
		if (IsActive && CanMove)
		{
			healthBar.SetActive(true);
			Vector2 newPos = Vector2.MoveTowards(rb2d.position, player.position, moveSpeed * Time.fixedDeltaTime);
			newPos.y = transform.position.y;
			transform.position = newPos;
		}
	}

	public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}
	void Update()
	{
		HasTarget = attackZone.detectedCollider.Count > 0;
		if (CanMove)
		{
			LookAtPlayer();
			MoveTowardsPlayer(speed);
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

	public void OnHit(float damage)
	{
		if (!isInvincible)
		{
			if (UnityEngine.Random.value < (hitProbability / 100f))
			{
				animator.SetTrigger("hit");
			}
			isInvincible = true;
			health -= damage;
			CharacterEvents.characterDamaged.Invoke(gameObject, damage);
		}
	}

	public void StartChasing()
	{
		IsActive = true;
	}

}