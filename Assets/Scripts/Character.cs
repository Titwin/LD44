using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public abstract class Character : Attackable
{
    public Weapon weapon;

    [SerializeField] GameObject bodyTemplate;

    protected virtual void Awake()
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

    protected override void OnDeath()
    {
        base.OnDeath();
        if (bodyTemplate)
        {
            GameObject body = GameObject.Instantiate<GameObject>(bodyTemplate);
            body.transform.position = this.transform.position;
            body.SetActive(true);
        }
    }

}
