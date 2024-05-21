using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCollider = new List<Collider2D>();
    public int colliderAmount;
    Collider2D col;
    
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        colliderAmount = detectedCollider.Count;
        foreach (Collider2D collider in detectedCollider)
        {
            if (!collider.GetComponent<TreeController>().IsGrown)
            {
                collider.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<TreeController>().IsGrown)
        {
            detectedCollider.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCollider.Remove(collision);
    }

}
