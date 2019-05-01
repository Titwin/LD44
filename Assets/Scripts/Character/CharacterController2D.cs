using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    public Rigidbody2D rb2D { get; private set; }
    public AnimationController ac;

    public Vector2 size;
    public float speed = 1;
    public float jumpSpeed = 1.0f;

    public bool canAttack = true;
    public float attackCooldownTime = 1;
    private IEnumerator cooldownCoroutine;

    float jumpFrames = -1;
    int groundedFilter = 0;

    int contactCount = 0;

    private ContactPoint2D[] contacts = new ContactPoint2D[32];

    [Header("State flags")]
    // these flags are serialized only for debug reasons
    [SerializeField] bool contactDown = false;
    [SerializeField] bool contactUp = false;
    [SerializeField] bool contactLeft = false;
    [SerializeField] bool contactRight = false;

    [Header("State flags")]
    // these values are resetted at the end of the frame, do not use after LateUpdate()
    [SerializeField] float movementX = 0;
    [SerializeField] bool jump = false;
    [SerializeField] bool duck = false;
    [SerializeField] bool attack = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void LateUpdate()
    {
        CheckContacts();
        float dx = movementX * speed;
        float dy = rb2D.velocity.y;
        bool grounded = IsGrounded();

        if (!ac) return;

        if (!canAttack)
        {
            ac.playAnimation(AnimationController.AnimationType.ATTACK);
        }
        else if (attack && canAttack)
        {
            cooldownCoroutine = AttackCooldown(attackCooldownTime);
            StartCoroutine(cooldownCoroutine);
            canAttack = false;
            ac.playAnimation(AnimationController.AnimationType.ATTACK);
        }
        else if (canAttack)
        {
            if (dx > 0)
            {
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (dx < 0)
            {
                this.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            
            if (contactLeft && dx < 0)
            {
                dx = 0;
            }
            else if (contactRight && dx > 0)
            {
                dx = 0;
            }


            if (grounded)
            {
                // JUMP PREPARE
                if (jumpFrames > 0)
                {
                    jumpFrames -= Time.deltaTime;
                    if (jumpFrames <= 0)
                    {
                        dy = jumpSpeed;
                        
                        ac.playAnimation(AnimationController.AnimationType.JUMPPREPARE);
                    }
                    else
                    {
                        ac.playAnimation(AnimationController.AnimationType.JUMPPREPARE);
                    }
                }
                else if (jump)
                {
                    jumpFrames = 0.02f;
                    ac.playAnimation(AnimationController.AnimationType.JUMPPREPARE);
                }

                //  DUCK
                else if (duck)
                {
                    dx = 0;
                    ac.playAnimation(AnimationController.AnimationType.DUCKING);
                }

                //  WALK
                else if (dx != 0)
                {
                    ac.playAnimation(AnimationController.AnimationType.WALKING);
                }

                //  IDLE
                else
                {
                    ac.playAnimation(AnimationController.AnimationType.IDLE);
                }
            }
            else
            {
                //  JUMP
                if (rb2D.velocity.y > 0)
                {
                    ac.playAnimation(AnimationController.AnimationType.JUMPING);
                }

                //  FALL
                else
                {
                    ac.playAnimation(AnimationController.AnimationType.FALLING);
                }
            }

            if (dx > 0)
            {
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (dx < 0)
            {
                this.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }


        rb2D.AddForce(new Vector2(dx, dy) - rb2D.velocity, ForceMode2D.Impulse);
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
    public void Push(Vector2 force)
    {
        rb2D.AddForce(force, ForceMode2D.Impulse);
    }

    void CheckContacts()
    {
        contactDown = false;
        contactUp = false;
        contactLeft = false;
        contactRight = false;

        contactCount = rb2D.GetContacts(contacts);
        for (int c = 0; c < contactCount; ++c)
        {
            if (contacts[c].collider.gameObject != this.gameObject && !contacts[c].collider.isTrigger)
            {
                if (contacts[c].point.y < transform.position.y - size.y / 2 + 0.01f)
                {
                    contactDown = true;
                }
                else if (contacts[c].point.y > transform.position.y + size.y / 2 - 0.01f)
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
        bool contact = (contactDown || (contactLeft && contactRight));
        if (contact)
            groundedFilter = 5;
        else
            groundedFilter--;
        return groundedFilter > 0;
    }
    public bool IsBlocked(int direction)
    {
        if (direction == 1) return contactRight;
        else if (direction == -1) return contactLeft;
        else return false;
    }

    private IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(this.transform.position, size);
        Gizmos.color = Color.green;
        if (contacts != null)
        {
            for (int c = 0; c < contactCount; ++c)
            {
                if (contacts[c].collider!=null && contacts[c].collider.gameObject != this.gameObject)
                {
                    Gizmos.DrawSphere(contacts[c].point, 0.1f);
                    // Gizmos.DrawLine(contacts[c].collider.transform.position, this.transform.position);
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
}
