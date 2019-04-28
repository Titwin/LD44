using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public abstract class Character : Attackable
{
    public Weapon weapon;

    [SerializeField] GameObject bodyTemplate;

    protected virtual void Start()
    {
        if (health == null)
        {
            Debug.LogError(name + " has no health");
        }

        if (weapon == null)
        {
            Debug.LogError(name + " has no weapon");
        }

        
    }

    public bool Attack()
    {
        if (weapon.CanAttack())
        {
            weapon.DoAttack();
            return true;
        }
        else { return false; }
    }

    protected override void OnDeath(GameObject source)
    {
        base.OnDeath(source);
        if (bodyTemplate != null)
        {
            GameObject body = GameObject.Instantiate<GameObject>(bodyTemplate);
            body.transform.position = this.transform.position;
            body.SetActive(true);
        }
    }
}
