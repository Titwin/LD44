using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    public AttackCollider attackCollider;

    public override void DoAttack()
    {
        if (CanAttack())
        {
            base.DoAttack();

            {
                foreach (var target in attackCollider.InRange)
                {
                    target.DoDamage(this.owner, this.damages);
                }
            }
        }
    }

    public override bool InRange(Attackable target)
    {
        return attackCollider.InRange.Contains(target);
    }
}
