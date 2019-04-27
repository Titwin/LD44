using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb;
    public AnimationController ac;

    private Character character;

    public Vector2 size;
    public float speed = 1;
    public float jumpSpeed = 1.0f;

    public bool canAttack = true;
    public bool isInteracting = false;
    public float attackCooldownTime = 1;
    private IEnumerator cooldownCoroutine;
    [SerializeField] Interactable interactable;

    private float distToGround;
    
    int contactCount = 0;
    private ContactPoint2D[] contacts = new ContactPoint2D[32];
    [Header("State flags")]

    // these flags are serialized only for debug reasons
    [SerializeField] bool contactDown = false;
    [SerializeField] bool contactUp = false;
    [SerializeField] bool contactLeft = false;
    [SerializeField] bool contactRight = false;
    [SerializeField] int direction = 1;

    // these values are resetted at the end of the frame, do not use after LateUpdate()
    float movementX = 0;
    bool jump = false;
    bool duck = false;
    bool attack = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        distToGround = GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f;
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        CheckContacts();
        if(!canAttack)
        {
            ac.playAnimation(AnimationController.AnimationType.ATTACK);//, attackCooldownTime/animationAttack.Length);
        }
        else if (attack && canAttack)
        {
            cooldownCoroutine = AttackCooldown(attackCooldownTime);
            StartCoroutine(cooldownCoroutine);
            canAttack = false;
            ac.playAnimation(AnimationController.AnimationType.ATTACK);//, attackCooldownTime / animationAttack.Length);
        }
        else if(canAttack)
        {
            float dx = movementX * speed;

            if (dx > 0)
            {
                direction = 1;
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (dx < 0)
            {
                direction = -1;
                this.transform.localEulerAngles = new Vector3(0, 180, 0);
            }

            //float dy = 0.0f;
            if (contactLeft && dx < 0)
            {
                dx = 0;
            }
            else if (contactRight && dx > 0)
            {
                dx = 0;
            }


            if(IsGrounded())
            {
                if(jump)
                {
                    rb.velocity = new Vector2(dx, jumpSpeed);
                    ac.playAnimation(AnimationController.AnimationType.INAIR);//, 0.2f);
                }
                else if(duck)
                {
                    rb.velocity = new Vector2(dx / 2, 0);
                    ac.playAnimation(AnimationController.AnimationType.DUCKING);//, 0.2f);

                    if(interactable!= null && interactable.CanInteract(this.character))
                    {
                        StartCoroutine(DoInteraction(interactable));
                    }
                }
                else if(dx != 0)
                {
                    rb.velocity = new Vector2(dx, 0);
                    ac.playAnimation(AnimationController.AnimationType.WALKING);//, 0.1f);
                }
                else
                {
                    ac.playAnimation(AnimationController.AnimationType.IDLE);//, 0.9f);
                }
            }
            else
            {
                rb.velocity = new Vector2(dx, rb.velocity.y);
                ac.playAnimation(AnimationController.AnimationType.INAIR);//, 0.2f);
            }
            
            if (dx > 0)
            {
                direction = 1;
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (dx < 0)
            {
                direction = -1;
                this.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
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
                if (contacts[c].point.y < transform.position.y - size.y/2+0.01f)
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
        //return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1);
        RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up* distToGround, -Vector2.up, 0.1f);
        if (hit.collider != null && hit.collider.gameObject!=this.gameObject)
        {
            return true;
        }
        return false;
        //return contactDown || (contactLeft && contactRight);
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
    private IEnumerator DoInteraction(Interactable interactable)
    {
        if (!isInteracting)
        {
            isInteracting = true;
            interactable.DoInteract(character);
            yield return new WaitForSeconds(interactable.interactionDuration);
            isInteracting = false;
        }
    }
    internal void ExitedInteractable(Interactable _interactable)
    {
        if (this.interactable == _interactable)
            interactable = null;
    }

    internal void EnteredInteractable(Interactable _interactable)
    {
        this.interactable = _interactable;
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, size);
        Gizmos.color = Color.green;
        for (int c = 0; c < contactCount; ++c)
        {
            if (contacts[c].collider.gameObject != this.gameObject)
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
