using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBossActive : MonoBehaviour
{
	[SerializeField]
	DeathEnemy deathEnemy;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			deathEnemy.IsActive = true;
		}
	}
}
