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
        if (collision != null && enemy)
        {
            enemy.OnHit(attackDamage, knockBack);
        }
    }
}
