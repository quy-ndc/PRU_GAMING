using UnityEngine;

public class Laser : MonoBehaviour
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
	}
}
