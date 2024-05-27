using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage;
    public Vector2 knockBack = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GroundEnemyController enemy = collision.GetComponent<GroundEnemyController>();
        Boss_Health boss = collision.GetComponent<Boss_Health>();
        if (collision != null && enemy)
        {
            enemy.OnHit(attackDamage, knockBack);
        }
        else if (collision != null && boss) 
        {
            boss.TakeDamage(attackDamage);
        }
    }
}
