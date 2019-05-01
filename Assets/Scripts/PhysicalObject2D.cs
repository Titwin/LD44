using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicalObject2D : MonoBehaviour
{
    private static readonly int MinGroundedTime = 5;
    
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] Vector2 size = Vector2.one;
    
    [Header("State flags")]
    // these flags are serialized only for debug reasons
    [SerializeField] bool contactDown = false;
    [SerializeField] bool contactUp = false;
    [SerializeField] bool contactLeft = false;
    [SerializeField] bool contactRight = false;

    private int groundedFilter = 0;
    private int contactCount;
    private ContactPoint2D[] contacts = new ContactPoint2D[32];

    private void Start()
    {
        if(!rb2D)rb2D = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        CheckContacts();
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
            groundedFilter = MinGroundedTime;
        else
            groundedFilter--;
        return groundedFilter > 0;
    }
    public bool IsBlockedX(int direction)
    {
        if (direction == 1) return contactRight;
        else if (direction == -1) return contactLeft;
        else return false;
    }
    public bool IsBlockedY(int direction)
    {
        if (direction == 1) return contactUp;
        else if (direction == -1) return contactDown;
        else return false;
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
                if (contacts[c].collider != null && contacts[c].collider.gameObject != this.gameObject)
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
