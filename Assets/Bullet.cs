using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int minDamage;
	public int maxDamage;
	public Vector2 knockBack;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		DamagePlayer();
	}

	void DamagePlayer()
	{
		int damage = UnityEngine.Random.Range(minDamage, maxDamage);
		PlayerController.Instance.OnHit(damage, knockBack);
		Destroy(gameObject);
	}
}
