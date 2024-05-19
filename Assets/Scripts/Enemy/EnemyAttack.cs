using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage;
    public Vector2 knockBack;
    [SerializeField]
    GroundEnemyController enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<GroundEnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.GetComponent<PlayerController>() && !PlayerController.Instance.IsBlocking)
        {
            PlayerController.Instance.OnHit(attackDamage, knockBack);
        }

        if (collision != null && collision.GetComponent<PlayerController>() && PlayerController.Instance.IsBlocking)
        {
           // enemy.OnHit(0, knockBack);
            PlayerController.Instance.OnBlocked(attackDamage, knockBack);
        }

    }
}
