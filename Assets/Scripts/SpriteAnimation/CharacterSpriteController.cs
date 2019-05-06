using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : SpriteAnimatorController
{
    [SerializeField] PhysicalObject2D PO2D;
    #region state variables
    [SerializeField] Vector2 speed = Vector2.one;
    [SerializeField] int direction = 0;
    [SerializeField] bool ducking = false;
    [SerializeField] bool grounded = true;
    #endregion


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
        bool nowGrunded = PO2D.rb2D.velocity.y == 0;//hack
        UpdateStateMachine(dx, jump, attack, crouch, PO2D.IsGrounded());
    }

    private void UpdateStateMachine(float dxInput, bool jump, bool attack, bool crouch, bool nowGrunded)
    {
        Vector2 velocity = PO2D.rb2D.velocity;
        int dx = 0;
        if (dxInput != 0) dx = (int)Mathf.Sign(dxInput);
        if (PO2D.IsBlockedX(dx)) dx = 0;
        if (!this.Waiting)
        {
            // attack has priority over everything
            if (attack)
            {
                velocity.x = nowGrunded ? 0 : velocity.x;
                SetActiveAnimation("attack1", true);
                Waiting = true;
            }
            else
            {   // then nongrounded actions, falling and flying
                if (!nowGrunded)
                {
                    if (PO2D.rb2D.velocity.y > 0)
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
                        SetActiveAnimation("jump1", true);
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
                            this.Reset |= ducking && !nowDucking;
                            if (this.Reset || dx != direction)
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
                            velocity.x = dx * speed.x;
                        }

                    }
                }
            }
            this.grounded = nowGrunded;
            if (PO2D.rb2D.velocity != velocity)
            {
                Vector2 delta = (velocity - PO2D.rb2D.velocity);
                PO2D.rb2D.AddForce(delta, ForceMode2D.Impulse);
            }
            this.Reset = false;
        }
    }
}
