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
    public float deathAnimationDuration = 1.5f;

    private Animator animator;


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
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("die");
        StartCoroutine(DestroyAfterAnimation(deathAnimationDuration));
    }

    private IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDame(20);
        }
    }

}
