using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCollider = new List<Collider2D>();
    public int colliderAmount;

    private void Update()
    {
        colliderAmount = detectedCollider.Count;
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
