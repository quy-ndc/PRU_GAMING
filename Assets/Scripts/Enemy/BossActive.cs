using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActive : MonoBehaviour
{
    [SerializeField]
    public GameObject HeathBar;
    public bool isActive = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
            Boss.Instance.IsActive = true;
            HeathBar.SetActive(true) ;
        }
    }
}
