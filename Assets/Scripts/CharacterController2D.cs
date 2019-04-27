using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public float speed = 1;
    public float jumpSpeed = 1.0f;

    public bool canAttack = true;
    public float attackCooldownTime = 1;
    private IEnumerator cooldownCoroutine;

    private float distToGround;

    int contactCount = 0;
    private ContactPoint2D[] contacts = new ContactPoint2D[32];
    [Header("State flags")]
    // these flags are serialized only for debug reasons
    [SerializeField] bool contactDown = false;
    [SerializeField] bool contactUp = false;
    [SerializeField] bool contactLeft = false;
    [SerializeField] bool contactRight = false;

    // these values are resetted at the end of the frame, do not use after LateUpdate()
    float movementX = 0;
    bool jump = false;
    bool duck = false;
    bool attack = false;

    // animator hash values
    int idleHash;
    int walkingHash;
    int attackingHash;
    int duckingHash;
    int inAirHash;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        distToGround = GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f;

        int jumpHash = Animator.StringToHash("Jump");
        int runStateHash = Animator.StringToHash("Base Layer.Run");

        idleHash = Animator.StringToHash("idle");
        walkingHash = Animator.StringToHash("walking");
        attackingHash = Animator.StringToHash("attacking");
        duckingHash = Animator.StringToHash("ducking");
        inAirHash = Animator.StringToHash("inAir");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        animator.ResetTrigger(idleHash);
        animator.ResetTrigger(walkingHash);
        animator.ResetTrigger(attackingHash);
        animator.ResetTrigger(duckingHash);
        animator.ResetTrigger(inAirHash);

        CheckContacts();
        {
            float dx = movementX * speed;
            if (contactLeft && dx < 0)
            {
                dx = 0;

            }
            else if (contactRight && dx > 0)
            {
                dx = 0;
            }
            if (IsGrounded() && jump)
            {
                rb.velocity = new Vector2(dx, jumpSpeed);
                animator.SetTrigger(inAirHash);
            }
            else if (IsGrounded() && duck)
            {
                rb.velocity = new Vector2(dx / 2, 0);
                animator.SetTrigger(duckingHash);
            }
            else
            {
                rb.velocity = new Vector2(dx, rb.velocity.y);
                if (rb.velocity == new Vector2(0, 0))
                {
                    animator.SetTrigger(idleHash);
                }
                else
                {
                    animator.SetTrigger(walkingHash);
                }
            }
        }
        /*else
        {
            animator.SetTrigger(inAirHash);
            //rb.velocity = new Vector2(dx, rb.velocity.y);
        }*/
        if (attack && canAttack)
        {
            cooldownCoroutine = AttackCooldown(attackCooldownTime);
            StartCoroutine(cooldownCoroutine);
            canAttack = false;
        }
        if (!canAttack)
        {
            animator.SetTrigger(attackingHash);
        }

        // reset controls
        movementX = 0;
        jump = false;
        duck = false;
        attack = false;
    }

    public void Move(float movementX, bool jump, bool attack, bool duck)
    {
        this.movementX = movementX;
        this.jump = jump;
        this.attack = attack;
        this.duck = duck;
    }
    void CheckContacts()
    {
        contactDown = false;
        contactUp = false;
        contactLeft = false;
        contactRight = false;

        contactCount = rb.GetContacts(contacts);
        for (int c = 0; c < contactCount; ++c)
        {
            if (contacts[c].collider.gameObject != this.gameObject)
            {
                if (contacts[c].point.y < transform.position.y - 0.49f)
                {
                    contactDown = true;
                }
                else if (contacts[c].point.y > transform.position.y + 0.49f)
                {
                    contactUp = true;
                }
                else if (contacts[c].point.x > transform.position.x)
                {
                    contactRight = true;
                }
                else if (contacts[c].point.x < transform.position.x)
                {
                    contactLeft = true;
                }
            }
        }

    }
    public bool IsGrounded()
    {
        //return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1);
        /*RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up* distToGround, -Vector2.up, 0.1f);
        if (hit.collider != null && hit.collider.gameObject!=this.gameObject)
        {
            return true;
        }
        return false;*/
        return contactDown || (contactLeft && contactRight);
    }

    private IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int c = 0; c < contactCount; ++c)
        {
            if (contacts[c].collider.gameObject != this.gameObject)
            {
                Gizmos.DrawSphere(contacts[c].point, 0.1f);
                Gizmos.DrawLine(contacts[c].collider.transform.position, this.transform.position);
            }
        }
        Gizmos.color = Color.red;
        if (contactDown)
        {
            Gizmos.DrawSphere(this.transform.position - new Vector3(0, 0.5f, 0), 0.1f);
        }
        if (contactUp)
        {
            Gizmos.DrawSphere(this.transform.position + new Vector3(0, 0.5f, 0), 0.1f);
        }
        if (contactRight)
        {
            Gizmos.DrawSphere(this.transform.position + new Vector3(0.5f, 0, 0), 0.1f);
        }
        if (contactLeft)
        {
            Gizmos.DrawSphere(this.transform.position - new Vector3(0.5f, 0, 0), 0.1f);
        }
    }
}
