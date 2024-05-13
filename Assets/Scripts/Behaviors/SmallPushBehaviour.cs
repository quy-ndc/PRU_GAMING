using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPushBehaviour : StateMachineBehaviour
{
    public float pushForce;
    public Rigidbody2D pushedObject;
    public float pushTime;
    private float pushTimer;
    float originalGravity;
    TouchingDirection td;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pushTimer = 0f;
        pushedObject = animator.GetComponent<Rigidbody2D>();
        originalGravity = pushedObject.gravityScale;
        td = pushedObject.GetComponent<TouchingDirection>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pushTimer += Time.deltaTime;
        if (!(pushTimer > pushTime) && td.IsGrounded)
        {
            pushedObject.velocity = new Vector2(pushedObject.transform.localScale.x * pushForce, 0f);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pushedObject.gravityScale = originalGravity;
    }
}
