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
    }
    public void TakeDamage(float damage)
    {
        if (!isInvulnerable)
        {
            animator.SetBool("TakeDamage", true);
            health -= damage;
            Canvas.GetComponentInChildren<TextMeshProUGUI>().text = health + "/" + Maxhealth;
            healthBarFill.UpdateBar(health, Maxhealth);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
        }

        if (health <= Maxhealth / 2)
        {
            Boss boss = GetComponent<Boss>();
            boss.speed *= 1.06f;
            Boss_weapon weapon = GetComponent<Boss_weapon>();
            weapon.attackRange *= 1.05f;
            weapon.attackDamage *= 1.07f;
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;

            foreach (var clip in overrideController.animationClips)
            {
                if (clip.name == "d_cleave")
                {
                    AnimationClip newClip = Instantiate(clip);
                    newClip.frameRate = clip.frameRate * 4f;
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
