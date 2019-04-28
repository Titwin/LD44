using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : TriggerCollider
{
    public List<Attackable> InRange { get; private set; }
    public LayerMask attackableMask;

    [SerializeField] int count = 0;
    protected override void Awake()
    {
        base.Awake();

        InRange = new List<Attackable>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (attackableMask == (attackableMask | (1 << other.gameObject.layer)))
        {
            var hitBox = other.GetComponent<HitCollider>();
            if (hitBox != null)
            {
                InRange.Add(hitBox.character);
                ++count;
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (attackableMask == (attackableMask | (1 << other.gameObject.layer)))
        {
            var hitBox = other.GetComponent<HitCollider>();
            if (hitBox != null)
            {
                InRange.Remove(hitBox.character);
                --count;
            }
        }
    }
}
