using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Boss_Health : MonoBehaviour
{
    public float Maxhealth = 500;
    public float health = 500;
    public bool isInvulnerable = false;
    [SerializeField]
    public GameObject Canvas;
    public HeathBar healthBarFill;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
    }
    public void TakeDamage(float damage)
    {
        if (isInvulnerable)
        {
            Debug.Log("Boss is invulnerable, damage not applied.");
            return;
        }
        animator.SetTrigger("TakeDamage");
        health -= damage;
        Canvas.GetComponentInChildren<TextMeshProUGUI>().text = health + "/" + Maxhealth;
        healthBarFill.UpdateBar(health, Maxhealth);
        Debug.Log($"Boss took {damage} damage, current health: {health}");
        CharacterEvents.characterDamaged.Invoke(gameObject, damage);


        if (health <= Maxhealth / 2)
        {
            Boss boss = GetComponent<Boss>();
            boss.speed *= 1.05f;
            Boss_weapon weapon = GetComponent<Boss_weapon>();
            weapon.attackRange *= 1.04f;
            weapon.attackDamage *= 1.06f;
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;

            foreach (var clip in overrideController.animationClips)
            {
                if (clip.name == "d_cleave")
                {
                    AnimationClip newClip = Instantiate(clip);
                    newClip.frameRate = clip.frameRate * 3f;
                    overrideController[clip.name] = newClip;
                    break;
                }

            }

            if (health <= 0)
            {
                IsAlive = false;

            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool("isAlive", value);
        }
    }


    void Die()
    {
        Destroy(gameObject);
        Destroy(Canvas);
    }

}
