using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_weapon : MonoBehaviour
{
    public float attackDamage = 20;
    //public int enragedAttackDamage = 40;
    public Vector2 knockback;
    public Vector3 attackOffset;
    public float attackRange ;
    public LayerMask attackMask;
    PlayerController player;
    
    public void Attack(Collider2D collision)
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        collision = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (collision != null)
        {
            player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.OnHit(attackDamage, knockback);
            }
        }
        else return;
        Debug.Log("Attack event received");

    }







    /*  public void EnragedAttack()
      {
          Vector3 pos = transform.position;
          pos += transform.right * attackOffset.x;
          pos += transform.up * attackOffset.y;

          Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
          if (colInfo != null)
          {
             colInfo.GetComponent<PlayerHealth>().TakeDamage(enragedAttackDamage);
          }
      }*/

    void OnDrawGizmosSelected()
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;

            Gizmos.DrawWireSphere(pos, attackRange);
        }
    
}