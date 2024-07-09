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
        Boss bossSlime = collision.GetComponent<Boss>();
        MechaGolem bossMecha = collision.GetComponent<MechaGolem>();
        DeathSummon deathSummon = collision.GetComponent<DeathSummon>();
        DeathEnemy deathEnemy = collision.GetComponent<DeathEnemy>();
        if (collision != null && enemy)
        {
            enemy.OnHit(attackDamage, knockBack);
        }
        if (collision != null && bossSlime) 
        {
            Boss.Instance.OnHit(attackDamage);
        }
        if (collision != null && bossMecha)
        {
            bossMecha.OnHit(attackDamage, knockBack);
        }
        if (collision != null && deathSummon)
        {
            deathSummon.OnHit();
        }
        if (collision != null && deathEnemy)
        {
            deathEnemy.OnHit(attackDamage, knockBack);
        }
    }
}
