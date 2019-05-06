using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimatorController : MonoBehaviour
{
    [SerializeField] protected SpriteAnimator animator;

    [Header("State variables (debug)")]
    [SerializeField] string previousAnimation;
    [SerializeField] string currentAnimation;
    [SerializeField] bool waiting = false;
    [SerializeField] bool reset = true;

    protected bool Waiting
    {
        get { return waiting; }
        set { waiting = value; }
    }
    protected bool Reset
    {
        get { return reset; }
        set { reset = value; }
    }
    protected virtual void OnEnable()
    {
        animator.OnAnimationStart += this.OnAnimationStart;
        animator.OnAnimationMinDurationReached += this.OnAnimationMinDurationReached;
        animator.OnAnimationLoop += this.OnAnimationLoop;
        animator.OnAnimationEnd += this.OnAnimationEnd;
    }
    protected virtual void OnDisable()
    {
        animator.OnAnimationStart -= this.OnAnimationStart;
        animator.OnAnimationMinDurationReached -= this.OnAnimationMinDurationReached;
        animator.OnAnimationLoop -= this.OnAnimationLoop;
        animator.OnAnimationEnd -= this.OnAnimationEnd;
    }

    public virtual void OnAnimationStart(SpriteAnimation animation)
    {
        previousAnimation = currentAnimation;
        currentAnimation = animation.name;
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
    
    public bool IsActiveAnimation(string animationName)
    {
        return animationName == currentAnimation;
    }
    public void SetActiveAnimation(string animationName, bool reset = false)
    {
        if(reset || !IsActiveAnimation(animationName))
        {
            animator.PlayAnimationIndexed(animationName);
        }
    }
}
