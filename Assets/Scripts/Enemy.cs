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
    [SerializeField] float jumpiness = 0;
    [Header("Decision parameters")]
    [SerializeField] float pursuitRange = 0;
    [SerializeField] AnimationCurve agressionCurve;

    [Header("Decision flags (debug)")]
    // move to local variables
    [SerializeField] bool scared = false;
    [SerializeField] bool inAttackRange = false;
    [SerializeField] bool inPursuitRange = false;

    public enum AIState
    {
        idle, patrol, pursuit, attack, scape, heal
    }
    [Header("Init and state values")]
    [SerializeField] AIState state = AIState.idle;
    [SerializeField] AIState nextState;

    [SerializeField] Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(this.transform.position, Player.thePlayer.transform.position);
        //AIState nextState;

        scared = Random.value > agressionCurve.Evaluate(health.Ratio);
        inAttackRange = attackCollider.InRange.Contains(Player.thePlayer);
        inPursuitRange = distanceToPlayer < pursuitRange;
        
        // if it is a coward, it will try to  move away
        if (scared)
        {
            // move away
            nextState = AIState.scape;
        }
        // if it can attack, it will attack
        else if (inAttackRange && controller.canAttack)
        {
            // attack
            nextState = AIState.attack;
        }
        // if it cannot attack, but the player is in range, it will chase it up if it makes sense
        else if (inPursuitRange)
        {
            // pursuit
            nextState = AIState.pursuit;
            bool jump = Player.thePlayer.transform.position.y > this.transform.position.x && Random.value < jumpiness;
            float moveNoise = Random.Range(0.8f, 1.2f);
            controller.Move(Mathf.Sign(Player.thePlayer.transform.position.x - this.transform.position.x)* moveNoise, jump, false, false);
        }
        else
        {
            // patrol
            nextState = AIState.patrol;
        }
    }
}
