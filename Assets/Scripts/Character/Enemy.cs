using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Enemy : Character
{
    [SerializeField] CharacterController2D controller;
    [Header("Type parameters")]
    //[SerializeField] bool speed = false;
    //[SerializeField] bool floating = false;
    //[SerializeField] bool transparent = true;
    [SerializeField] float jumpiness = 0;
    [Header("Decision parameters")]
    [SerializeField] float thinkTime = 0.01f;
    [SerializeField] float pursuitRange = 0;
    [SerializeField] float patrolRange = 0;
    [SerializeField] float runawayRange = 0.1f;
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
    [Header("Init and state values (debug)")]
    [SerializeField] AIState state = AIState.idle;
    [SerializeField] AIState nextState;
    [SerializeField] float dx = 0;
    [SerializeField] float currentDX = 0;
    [SerializeField] bool doAttack = false;
    [SerializeField] bool doJump = false;
    [SerializeField] Vector3 startPoint;
    [SerializeField] int patrolDirection;
    [SerializeField] float t;
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        startPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime*Random.Range(0.7f,1.3f);
        if(t>= thinkTime)
        {
            t -= thinkTime;
            Think();
        }
        else
        {
            currentDX = Mathf.MoveTowards(currentDX, dx, Time.deltaTime*5);
        }
        if (doAttack)
        {
            weapon.DoAttack();
        }
        controller.Move(currentDX, doJump, doAttack, false);
    }

    void Think()
    {
        float signedDistanceX = Player.thePlayer.transform.position.x - this.transform.position.x;
        float distanceToPlayerXY = Vector2.Distance(this.transform.position, Player.thePlayer.transform.position);

        scared = Random.value > agressionCurve.Evaluate(health.Ratio);
        inAttackRange = weapon.InRange(Player.thePlayer);
        inPursuitRange = distanceToPlayerXY < pursuitRange;

        dx = 0;
        doAttack = false;
        doJump = false;
        if (inPursuitRange)
        {
            // if it is a coward, it will try to  move away
            if (scared || Mathf.Abs(signedDistanceX) < runawayRange)
            {
                // move away
                nextState = AIState.scape;
                dx = -Mathf.Sign(signedDistanceX);
            }
            // if it can attack, it will attack
            else if (inAttackRange)
            {
                // attack
                nextState = AIState.attack;
                dx = 0;
                doAttack = true;
            }
            // if it cannot attack, but the player is in range, it will chase it up if it makes sense
            else
            {

                // pursuit
                nextState = AIState.pursuit;
                doJump = Player.thePlayer.transform.position.y-Player.thePlayer.Controller.size.y/2 > this.transform.position.x - this.controller.size.y / 2 && Random.value < jumpiness;
                dx = Mathf.Sign(signedDistanceX);
            }
        }
        else
        {
            if (state != AIState.patrol)
            {
                startPoint = this.transform.position;
                patrolDirection = Random.value > 0.5f ? 1 : -1;
            }
            // patrol
            nextState = AIState.patrol;
            if (Mathf.Abs(this.transform.position.x - startPoint.x) >= patrolRange)
            {
                patrolDirection = -patrolDirection;
            }
            dx = patrolDirection;
        }
        doJump |= controller.IsBlocked((int)Mathf.Sign(dx));
        dx *= Random.Range(0.8f, 1.2f);
       

        state = nextState;
    }
}
