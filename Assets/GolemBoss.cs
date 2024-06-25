using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MechaGolem : MonoBehaviour
{
    private Animator animator;
    private Transform target;
    public float meleeRange = 4f;
    public float speed;
    public bool isFaceleft = true;
    private Rigidbody2D rb2d;

    public GameObject bullet;
    public Transform firePos;
    public float TimeBtwFire = 0.2f;
    public float bulletForce;
    private float timeBtwFire;

    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    public float laserDuration = 2.0f;
    public float laserSpeed = 30.0f;

    private bool isMoving = false;

    PlayerController playerController;
    MechaGolem_Heal mechaGolem_Heal;
    public int minDamage;
    public int maxDamage;
    public Vector2 knockBack;
    private void OnTriggerEnter2D(Collider2D collision)
    {/*
        if (collision.CompareTag("Player"))
        {
            playerController = collision.GetComponent<PlayerController>();
            InvokeRepeating("DamagePlayer", 0, 1f);
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if (collision.CompareTag("Player"))
        {
            playerController = null;
            CancelInvoke();
        }*/
    }

    void DamagePlayer()
    {
        int damage = UnityEngine.Random.Range(minDamage, maxDamage);
        playerController.OnHit(damage, knockBack);
    }
    public void OnHit(int damage)
    {
        mechaGolem_Heal.TakeDame(damage);
        CharacterEvents.characterDamaged.Invoke(gameObject, damage);
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        Collider2D[] colliders = GetComponents<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        flip();
        var distance = Vector2.Distance(transform.position, target.position);

        if (isMoving)
        {
            MoveTowardsPlayer(speed);
        }

        if (distance <= meleeRange)
        {
            isMoving = false;
            animator.SetTrigger("meleeAttack");
        }
        else
        {
            int chance = Random.Range(0, 2);
            if (chance == 0)
            {
                timeBtwFire -= Time.deltaTime;
                if (timeBtwFire < 0)
                {
                    isMoving = false;
                    animator.SetTrigger("laserAttack");
                    StartCoroutine(FireLaser());
                }
            }
            else
            {
                timeBtwFire -= Time.deltaTime;
                if (timeBtwFire < 0)
                {
                    isMoving = false;
                    animator.SetTrigger("rangedAttack");
                    FireBullet();

                }
            }
        }
    }

    void MoveTowardsPlayer(float moveSpeed)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void flip()
    {
        Vector3 movementDirection = target.position - transform.position;
        if (movementDirection.x > 0 && !isFaceleft)
        {
            isFaceleft = true;
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
        else if (movementDirection.x < 0 && isFaceleft)
        {
            isFaceleft = false;
            Vector3 newScale = transform.localScale;
            newScale.x = -Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
    }

    void FireBullet()
    {
        timeBtwFire = TimeBtwFire;

        GameObject bulletTmp = Instantiate(bullet, firePos.position, Quaternion.identity);
        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        Vector2 direction = (target.position - firePos.position).normalized;
        rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);

        StartCoroutine(ResumeMovementAfterDelay());
    }

    IEnumerator FireLaser()
    {
        GameObject laser = Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);

        Vector2 direction = (target.position - laserSpawnPoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laser.transform.rotation = Quaternion.Euler(0, 0, angle);
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        rb.velocity = direction * laserSpeed;
        yield return new WaitForSeconds(laserDuration);
        Destroy(laser);
        StartCoroutine(ResumeMovementAfterDelay());
    }

    IEnumerator ResumeMovementAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); 
        isMoving = true;
    }
}
