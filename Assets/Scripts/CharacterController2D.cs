using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;

    public float speed = 1;
    public float jumpSpeed = 1.0f;

    public bool canAttack = true;
    public float attackCooldownTime = 1;
    private IEnumerator cooldownCoroutine;

    private float distToGround;

    int idleHash;
    int walkingHash;
    int attackingHash;
    int duckingHash;
    int inAirHash;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        distToGround = GetComponent<CircleCollider2D>().bounds.extents.y + 0.1f;

        int jumpHash = Animator.StringToHash("Jump");
        int runStateHash = Animator.StringToHash("Base Layer.Run");

        idleHash = Animator.StringToHash("idle");
        walkingHash = Animator.StringToHash("walking");
        attackingHash = Animator.StringToHash("attacking");
        duckingHash = Animator.StringToHash("ducking");
        inAirHash = Animator.StringToHash("inAir");
    }
	
	// Update is called once per frame
	void Update ()
    {
        animator.ResetTrigger(idleHash);
        animator.ResetTrigger(walkingHash);
        animator.ResetTrigger(attackingHash);
        animator.ResetTrigger(duckingHash);
        animator.ResetTrigger(inAirHash);

        float dx = Input.GetAxis("Horizontal") * speed;
        if (IsGrounded())
        {
            if(Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(dx, jumpSpeed);
                animator.SetTrigger(inAirHash);
            }
            else if (Input.GetButton("Duck"))
            {
                rb.velocity = new Vector2(dx / 2, 0);
                animator.SetTrigger(duckingHash);
            }
            else
            {
                rb.velocity = new Vector2(dx, rb.velocity.y);
                if(rb.velocity == new Vector2(0,0))
                {
                    animator.SetTrigger(idleHash);
                }
                else
                {
                    animator.SetTrigger(walkingHash);
                }
            }
        }
        else
        {
            animator.SetTrigger(inAirHash);
            rb.velocity = new Vector2(dx, rb.velocity.y);
        }

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            cooldownCoroutine = AttackCooldown(attackCooldownTime);
            StartCoroutine(cooldownCoroutine);
            canAttack = false;
        }
        if(!canAttack)
        {
            animator.SetTrigger(attackingHash);
        }
    }
    public bool IsGrounded()
    {
        //return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1);
        RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up* distToGround, -Vector2.up, 0.1f);
        if (hit.collider != null && hit.collider.gameObject!=this.gameObject)
        {
            return true;
        }
        return false;
    }

    private IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

}
