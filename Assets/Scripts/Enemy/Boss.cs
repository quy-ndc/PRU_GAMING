using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Boss : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    public float speed;
    private Rigidbody2D rb2d;
    public bool isFlipped = false;
    public float range;
    public float attackRange = 3f;
    public Boss boss;
    [SerializeField] 
    public GameObject Canvas;
    bool isAttacking = false; // Add this to your class variables
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        Collider2D[] colliders = GetComponents<Collider2D>();
        rb2d = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }

        if (player != null)
        {
            Debug.Log("Player found and target assigned.");
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player GameObject has the tag 'Player'.");
        }
    }
  public  void MoveTowardsPlayer(float moveSpeed, float maxRange)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= maxRange && !isAttacking)
        {
            Canvas.SetActive(true);
            Vector2 newPos = Vector2.MoveTowards(rb2d.position, player.position, moveSpeed * Time.fixedDeltaTime);
            newPos.y = transform.position.y;
            transform.position = newPos;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Vector2.Distance(player.position, rb2d.position) <= attackRange && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Attack");
        }

    }

    public void TheWalkingIsDead()
    {
        isAttacking = false;

    }
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    void Update()
    {
        LookAtPlayer();
        MoveTowardsPlayer(speed, range);
    }

}
