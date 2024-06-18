using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MechaGolem_Heal : MonoBehaviour
{
    [SerializeField] int maxHeal;
    int curHeal;
    public HeathBar healbar;
    public UnityEvent OnDeath;


    private void OnEnable()
    {
        OnDeath.AddListener(Death);
    }

    private void OnDisable()
    {
        OnDeath.RemoveListener(Death);
    }
    private void Start()
    {
        curHeal = maxHeal;
        healbar.UpdateBar(curHeal,maxHeal);
    }
    public void TakeDame(int damage)
    {
        curHeal -= damage;
        if(curHeal <= 0)
        {
            curHeal = 0;
            OnDeath.Invoke();

        }
        healbar.UpdateBar(curHeal, maxHeal);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TakeDame(20);
        }
    }

}
