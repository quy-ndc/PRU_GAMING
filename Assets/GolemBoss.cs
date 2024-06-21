using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MechaGolem : MonoBehaviour
{
    // Mecha Golem Boss
    private Animator animator;
    private Transform target;
    public float meleeRange = 1f;
    public float speed;
    public bool isFaceleft = true;
    private Rigidbody2D rb2d;

    // Bullet
    public GameObject bullet;
    public Transform firePos;
    public float TimeBtwFire = 0.2f;
    public float bulletForce;
    private float timeBtwFire;

    // Laser
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    public float laserDuration = 2.0f;
    public float laserSpeed = 30.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        Collider2D[] colliders = GetComponents<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (target != null)
        {
            Debug.Log("Player found and target assigned.");
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player GameObject has the tag 'Player'.");
        }
    }

    void Update()
    {
        flip();
        var distance = Vector2.Distance(transform.position, target.position);
        MoveTowardsPlayer(speed);

        if (distance <= meleeRange)
        {
            animator.SetTrigger("meleeAttack");
        }
        else if (distance > 9.0f)
        {
            int chance = Random.Range(0, 2);
            if (chance == 0)
            {
                timeBtwFire -= Time.deltaTime;
                if (timeBtwFire < 0)
                {
                    MoveTowardsPlayer(0);
                    animator.SetTrigger("laserAttack");
                    StartCoroutine(FireLaser());
                }
            }
            else
            {
                timeBtwFire -= Time.deltaTime;
                if (timeBtwFire < 0)
                {
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
    }
}
