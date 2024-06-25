using UnityEngine;

public class Laser : MonoBehaviour
{
    private PlayerController playerController;
    public int minDamage;
    public int maxDamage;
    public Vector2 knockBack;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController = collision.GetComponent<PlayerController>();
            DamagePlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController = null;
        }
    }

    void DamagePlayer()
    {
        int damage = UnityEngine.Random.Range(minDamage, maxDamage);
        playerController.OnHit(damage, knockBack);
    }
}
