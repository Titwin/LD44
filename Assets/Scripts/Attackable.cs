using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable: MonoBehaviour
{
    public Health health;

    public void DoDamage(Character source, int amount)
    {
        Debug.Log(this.name + " got attacked for " + amount + "dmg" + ", out of " + health.Value);
        if (!health.Invulnerable)
        {
            health.Value -= amount;

            if (health.Value <= 0)
            {
                OnDeath();
                StartCoroutine(DoDestroy());
            }
            else
            {
                OnHurt();
            }
        }
    }

    protected IEnumerator DoDestroy()
    {
        yield return new WaitForEndOfFrame();
        GameObject.Destroy(this.gameObject);
    }

    protected virtual void OnHurt()
    {

    }
    protected virtual void OnDeath()
    {

    }
}
