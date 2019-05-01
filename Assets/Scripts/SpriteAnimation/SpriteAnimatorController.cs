using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimatorController : MonoBehaviour
{
    [SerializeField] SpriteAnimator animator;
    [SerializeField] Rigidbody2D rb2D;

    [SerializeField] Vector2 speed = Vector2.one;
    #region state variables
    [Header("State variables (debug)")]
    [SerializeField] int direction = 0;
    [SerializeField] bool waiting = false;
    [SerializeField] bool reset = true;
    [SerializeField] bool ducking = false;
    [SerializeField] bool grounded = true;
    [SerializeField] string previousAnimation;
    [SerializeField] string currentAnimation;
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
        //Debug.Log("Started:" + animation.name);
        previousAnimation = currentAnimation;
        currentAnimation = animation.name;
    }
    public virtual void OnAnimationMinDurationReached(SpriteAnimation animation)
    {
        //Debug.Log("can be interrupted:" + animation.name);
        waiting = false;
    }
    public virtual void OnAnimationLoop(SpriteAnimation animation)
    {
        //Debug.Log("looped:" + animation.name);
    }
    public virtual void OnAnimationEnd(SpriteAnimation animation)
    {
        //Debug.Log("ended:" + animation.name);
        waiting = false;
        reset = true;
    }
    bool IsActiveAnimation(string animationName)
    {
        return animationName == currentAnimation;
    }
    void SetActiveAnimation(string animationName, bool reset = false)
    {
        if(reset || !IsActiveAnimation(animationName))
        {
            animator.PlayAnimationIndexed(animationName);
        }
    }

    private void Start()
    {
        SetActiveAnimation("idle");
    }

    private void Update()
    {
        float dx = Input.GetAxis("Horizontal");
        bool attack = Input.GetKey(KeyCode.Space);
        bool jump = Input.GetButton("Jump");
        bool crouch = Input.GetButton("Duck");
        if (crouch) dx = 0;
        bool nowGrunded = rb2D.velocity.y == 0;//hack
        UpdateStateMachine(dx, jump, attack, crouch, nowGrunded);
    }

    private void UpdateStateMachine(float dxInput, bool jump, bool attack, bool crouch, bool nowGrunded)
    {
        Vector2 velocity = rb2D.velocity;
        int dx = 0;
        if (dxInput != 0) dx = (int)Mathf.Sign(dxInput);
        if (!waiting)
        {
            // attack has priority over everything
            if (attack)
            {
                velocity.x = nowGrunded?0:velocity.x;
                SetActiveAnimation("attack1",true);
                waiting = true;
            }
            else
            {   // then nongrounded actions, falling and flying
                if (!nowGrunded) {
                    if(rb2D.velocity.y > 0)
                    {
                        SetActiveAnimation("jump2");
                    }
                    else
                    {
                        SetActiveAnimation("fall");
                    }

                    if (dx > 0)
                    {
                        velocity.x = dx * speed.x;
                        animator.spriteRenderer.flipX = false;
                    }
                    else if (dx < 0)
                    {
                        velocity.x = dx * speed.x;
                        animator.spriteRenderer.flipX = true;
                    }
                    
                }
                else if (nowGrunded && !this.grounded)
                {
                    SetActiveAnimation("land");
                }
                else
                {
                    // then jump
                    bool jumping = jump;
                    if (jumping)
                    {
                        SetActiveAnimation("jump1",true);
                        velocity.y = 1 * speed.y;
                    }
                    // then grounded actions
                    else
                    {
                        //starting by ducking
                        bool nowDucking = crouch;
                        ducking = IsActiveAnimation("crouch");
                        if (!ducking && nowDucking)
                        {
                            SetActiveAnimation("crouch");
                            ducking = nowDucking;
                        }
                        else
                        {   //then directional movements
                            reset |= ducking && !nowDucking;
                            if (reset || dx != direction)
                            {
                                if (dx > 0)
                                {
                                    SetActiveAnimation("run", true);
                                    animator.spriteRenderer.flipX = false;
                                }
                                else if (dx < 0)
                                {
                                    SetActiveAnimation("run", true);
                                    animator.spriteRenderer.flipX = true;
                                }
                                //finally, if nothing happened, it is idle
                                else
                                {
                                    SetActiveAnimation("idle");
                                }
                                direction = dx;
                            }
                            velocity.x = dx*speed.x;
                        }
                        
                    }
                }
            }
            this.grounded = nowGrunded;
            if (rb2D.velocity != velocity)
            {
                Vector2 delta = (velocity - rb2D.velocity);
                rb2D.AddForce(delta, ForceMode2D.Impulse);
            }
            reset = false;
        }
    }
}
