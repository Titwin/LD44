using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Enemy : Character
{
    [SerializeField] CharacterController2D controller;
    [Header("Type parameters")]
    [SerializeField] bool speed = false;
    [SerializeField] bool floating = false;
    [SerializeField] bool transparent = true;

    [Header("Decision parameters")]
    [SerializeField] float pursuitRange = 0;
    [SerializeField] AnimationCurve agressionCurve;

    Vector3 startPoint;

    public enum AIState
    {
        idle, patrol, pursuit, attack, scape, heal
    }
    [SerializeField] AIState state = AIState.idle;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(this.transform.position, Player.thePlayer.transform.position);
        AIState nextState;

        bool scared = false;
        bool inAttackRange = false;
        bool inPursuitRange = false;
        
        // if it is a coward, it will try to  move away
        if (scared)
        {
            // move away
        }
        // if it can attack, it will attack
        else if (inAttackRange && controller.canAttack)
        {
            // attack
        }
        // if it cannot attack, but the player is in range, it will chase it up if it makes sense
        else if (inPursuitRange)
        {
            // pursuit
        }
        else
        {
            // patrol
        }
    }
}
