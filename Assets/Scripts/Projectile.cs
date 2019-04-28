﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Character owner;

    [SerializeField] int damage = 1;
    //[SerializeField] float range = 3;

    public LayerMask attackableMask;
    

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (attackableMask == (attackableMask | (1 << other.gameObject.layer)))
        {
            Attackable target = other.GetComponent<Attackable>();
            if (target != null)
            {
                target.DoDamage(owner, this.damage);
            }
        }
    }
}
