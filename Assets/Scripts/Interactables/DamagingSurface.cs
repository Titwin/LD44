using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingSurface : MonoBehaviour
{
    [SerializeField] LayerMask targetMask;
    [SerializeField] int damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (targetMask == (targetMask | (1 << collision.gameObject.layer)))
        {
            Attackable target = collision.gameObject.GetComponent<Attackable>();
            if(target != null)
            {
                target.DoDamage(this.gameObject, damage);
            }
        }
    }
}
