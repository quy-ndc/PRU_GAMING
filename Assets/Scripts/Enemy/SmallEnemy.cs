using System.Collections;
using UnityEngine;

public class SmallEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int damage = 10; 
    private Transform player;

    public void SetTarget(Transform target)
    {
        player = target;
        StartCoroutine(MoveTowardsPlayer());
    }

    IEnumerator MoveTowardsPlayer()
    {
        while (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Small enemy hit the player!");
            Destroy(gameObject);
        }
    }
}
