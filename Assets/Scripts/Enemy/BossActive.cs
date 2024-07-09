using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActive : MonoBehaviour
{
    [SerializeField]
    public GameObject HeathBar;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Boss.Instance.IsActive = true;
            HeathBar.SetActive(true) ;
        }
    }
}
