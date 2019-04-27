using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : TriggerCollider
{
    public List<Character> InRange { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        InRange = new List<Character>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        var hitBox = other.GetComponent<HitCollider>();
        if (hitBox != null)
        {
            InRange.Add(hitBox.character);
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        var hitBox = other.GetComponent<HitCollider>();
        if (hitBox != null)
        {
            InRange.Remove(hitBox.character);
        }
    }
}
