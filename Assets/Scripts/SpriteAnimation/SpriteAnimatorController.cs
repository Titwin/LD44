using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimatorController : MonoBehaviour
{
    [SerializeField] SpriteAnimator animator;
    #region state variables
    public int direction = 0;
    public bool waiting = false;
    bool reset = true;
    #endregion

    private void OnEnable()
    {
        animator.OnAnimationStart += this.OnAnimationStart;
        animator.OnAnimationMinDurationReached += this.OnAnimationMinDurationReached;
        animator.OnAnimationLoop += this.OnAnimationLoop;
        animator.OnAnimationEnd += this.OnAnimationEnd;
    }
    private void OnDisable()
    {
        animator.OnAnimationStart -= this.OnAnimationStart;
        animator.OnAnimationMinDurationReached -= this.OnAnimationMinDurationReached;
        animator.OnAnimationLoop -= this.OnAnimationLoop;
        animator.OnAnimationEnd -= this.OnAnimationEnd;
    }

    public virtual void OnAnimationStart(SpriteAnimation animation)
    {
    }
    public virtual void OnAnimationMinDurationReached(SpriteAnimation animation)
    {
        waiting = false;
    }
    public virtual void OnAnimationLoop(SpriteAnimation animation)
    {     
    }
    public virtual void OnAnimationEnd(SpriteAnimation animation)
    {
        waiting = false;
        reset = true;
    }

    private void Start()
    {
        animator.PlayAnimationIndexed("idle");
    }

    private void Update()
    {
        UpdateStateMachine();
    }

    private void UpdateStateMachine()
    {
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.PlayAnimationIndexed("attack1");
                waiting = true;
            }
            float dxInput = Input.GetAxis("Horizontal");
            int dx = 0;
            if (dxInput != 0) dx = (int)Mathf.Sign(dxInput);
            if (reset || dx != direction)
            {
                if (dx > 0)
                {
                    animator.PlayAnimationIndexed("run");
                    animator.spriteRenderer.flipX = false;
                }
                else if (dx < 0)
                {
                    animator.PlayAnimationIndexed("run");
                    animator.spriteRenderer.flipX = true;
                }
                else
                {
                    animator.PlayAnimationIndexed("idle");
                }
                direction = dx;
            }
            float dy = Input.GetAxis("Vertical");
            
            reset = false;
        }
    }
}
