using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGravityBehaviour : StateMachineBehaviour
{
    Rigidbody2D rb;
    float originalGravity;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.gravityScale = originalGravity;
    }

}
