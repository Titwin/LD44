﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable: MonoBehaviour
{
    public Health health;
    [SerializeField] float destroyTime = 0.1f;
    public void DoDamage(Character source, int amount)
    {
        OnAttack(source!=null?source.gameObject:null);
        if (health != null)
        {
            Debug.Log(this.name + " got attacked for " + amount + "dmg" + ", out of " + health.Value);
            if (!health.Invulnerable)
            {
                health.Value -= amount;

                if (health.Value <= 0)
                {
                    OnDeath(source != null ? source.gameObject : null);
                    StartCoroutine(DoDestroy());
                }
                else
                {
                    OnHurt(source != null ? source.gameObject : null);
                }
            }
        }
    }

    protected IEnumerator DoDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        GameObject.Destroy(this.gameObject);
    }
    protected virtual void OnAttack(GameObject source)
    {

    }
    protected virtual void OnHurt(GameObject source)
    {

    }

    protected virtual void OnDeath(GameObject source)
    {

    }
}
