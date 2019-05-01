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
        Debug.Log("animation started:" + animation.name);
    }
    public virtual void OnAnimationLoop(SpriteAnimation animation)
    {
        Debug.Log("animation looped:" + animation.name);
    }
    public virtual void OnAnimationEnd(SpriteAnimation animation)
    {
        Debug.Log("animation ended:" + animation.name);
    }

    //Example action using the controller
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
