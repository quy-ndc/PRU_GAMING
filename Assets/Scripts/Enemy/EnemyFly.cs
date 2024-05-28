using System.Collections;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    public float detectRange = 10f; 
    public float moveSpeed = 2f; 
    public float attackRange = 1f; 
    public GameObject smallEnemyPrefab; 
    public Transform player; 
    public int health = 100; 

    private bool isDead = false; 

    void Update()
    {
        if (!isDead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectRange)
            {
                MoveTowardsPlayer();

                if (distanceToPlayer <= attackRange)
                {
                    AttackPlayer();
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        Debug.Log("Attack the player!");
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Enemy is dead!");
        for (int i = 0; i < 3; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * 0.5f;
            GameObject smallEnemy = Instantiate(smallEnemyPrefab, spawnPosition, Quaternion.identity);
            SmallEnemy smallEnemyScript = smallEnemy.GetComponent<SmallEnemy>();
            smallEnemyScript.SetTarget(player);
        }

        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
