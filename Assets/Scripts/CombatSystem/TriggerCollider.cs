using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerCollider : MonoBehaviour
{
    public Collider2D Collider { get; protected set; }

    public List<Collider2D> Triggering { get; private set; }

    protected virtual void Awake()
    {
        Triggering = new List<Collider2D>();

        Collider = GetComponent<Collider2D>();
        if (!Collider.isTrigger)
        {
            Debug.LogWarning("Collider is not trigger on TriggerCollider on GameObject " + gameObject.name);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Triggering.Add(other);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        Triggering.Remove(other);
    }
}
