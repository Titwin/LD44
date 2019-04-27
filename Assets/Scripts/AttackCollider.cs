﻿using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : TriggerCollider
{
    public List<IAttackable> InRange { get; private set; }
    public LayerMask attackableMask;
    protected override void Awake()
    {
        base.Awake();

        InRange = new List<IAttackable>();
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
            }
        }
    }
}
