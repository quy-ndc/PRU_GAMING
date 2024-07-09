using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAttack : MonoBehaviour
{
	public float attackDamage;
	public Vector2 knockBack;
	public DeathSummon deathSummon;

	private void Awake()
	{
		deathSummon = GetComponentInParent<DeathSummon>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision != null && collision.GetComponent<PlayerController>() && !PlayerController.Instance.IsBlocking)
		{
			PlayerController.Instance.OnHit(attackDamage, knockBack);
			deathSummon.OnHit();
		}

		if (collision != null && collision.GetComponent<PlayerController>() && PlayerController.Instance.IsBlocking)
		{
			PlayerController.Instance.OnBlocked(attackDamage, knockBack);
			deathSummon.OnHit();
		}

	}
}
