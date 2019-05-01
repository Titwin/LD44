using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimatorController : MonoBehaviour
{
    [SerializeField] SpriteAnimator animator;

    private void OnEnable()
    {
        animator.OnAnimationStart += this.OnAnimationStart;
        animator.OnAnimationLoop += this.OnAnimationLoop;
        animator.OnAnimationEnd += this.OnAnimationEnd;
    }
    private void OnDisable()
    {
        animator.OnAnimationStart -= this.OnAnimationStart;
        animator.OnAnimationLoop -= this.OnAnimationLoop;
        animator.OnAnimationEnd -= this.OnAnimationEnd;
    }

    public virtual void OnAnimationStart(SpriteAnimation animation)
    {

    }
    public virtual void OnAnimationLoop(SpriteAnimation animation)
    {

    }
    public virtual void OnAnimationEnd(SpriteAnimation animation)
    {

    }

    int idx = 0;
    public SpriteAnimation[] animationsToCycle;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            idx = (++idx)%animationsToCycle.Length;
            
            animator.PlayAnimation(animationsToCycle[idx]);
        }
    }
}
