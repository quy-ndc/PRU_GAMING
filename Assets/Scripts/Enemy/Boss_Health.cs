using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Health : MonoBehaviour
{
    public float health = 100;

    public GameObject deathEffect;

    public bool isInvulnerable = false;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
    }
    public void TakeDamage(float damage)
    {
        if (isInvulnerable)
        {
            Debug.Log("Boss is invulnerable, damage not applied.");
            return;
        }
        animator.SetTrigger("TakeDamage");
        health -= damage;
        Debug.Log($"Boss took {damage} damage, current health: {health}");
        CharacterEvents.characterDamaged.Invoke(gameObject, damage);

        /*  if (health <= 200)
          {
              GetComponent<Animator>().SetBool("IsEnraged", true);
          }*/
        
        if (health <= 0)
        {
            IsAlive = false;
            animator.SetTrigger("Die");
            StartCoroutine(DestroyAfterAnimation());
        }
    }
    private IEnumerator DestroyAfterAnimation()
    {
        // Assuming the death animation is 2 seconds long
        yield return new WaitForSeconds(2.2f);
        Die();
        // or gameObject.SetActive(false);
    }

    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool("isAlive", value);
        }
    }


    void Die()
    {
     //   animator.SetTrigger("Die");
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

}
