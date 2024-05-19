using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    public List<GameObject> detectedCollider = new List<GameObject>();
    public float gravityScale;

    private void Update()
    {
        foreach (GameObject obj in detectedCollider)
        {
            obj.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedCollider.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCollider.Remove(collision.gameObject);
    }
}
