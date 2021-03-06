﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRange : Weapon
{
    [SerializeField] Projectile projectileTemplate;
    [SerializeField] Vector2 orientation = new Vector2(1,0);
    [SerializeField] float speed = 10;
    [SerializeField] float delay;
    private IEnumerator delayCoroutine;


    public override bool InRange(Attackable target)
    {
        throw new System.NotImplementedException();
    }
    public override void DoAttack()
    {
        if (CanAttack())
        {
            StartCoroutine(launchProjectile());

            /*base.DoAttack();
            Projectile projectile = GameObject.Instantiate<Projectile>(projectileTemplate);
            projectile.owner = this.owner;
            Transform pt = projectile.transform;
            pt.transform.position = projectileTemplate.transform.position;
            pt.transform.rotation = projectileTemplate.transform.rotation;

            pt.gameObject.SetActive(true);
            projectile.rb.velocity = this.transform.TransformVector(orientation* speed);*/

        }
    }

    private IEnumerator launchProjectile()
    {
        base.DoAttack();
        Projectile projectile = GameObject.Instantiate<Projectile>(projectileTemplate);
        projectile.owner = this.owner;
        Transform pt = projectile.transform;
        pt.transform.position = projectileTemplate.transform.position;
        pt.transform.rotation = projectileTemplate.transform.rotation;

        pt.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        projectile.rb.velocity = this.transform.TransformVector(orientation * speed);
        Debug.DrawLine(projectile.transform.position, projectile.transform.position + new Vector3(projectile.rb.velocity.x, projectile.rb.velocity.y)*0.1f);
    }
}
