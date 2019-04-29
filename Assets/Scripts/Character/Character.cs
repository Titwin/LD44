using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public abstract class Character : Attackable
{
    public Weapon weapon;

    [SerializeField] GameObject bodyTemplate;

    public List<AudioClip> attackClips;

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
            audioSource.PlayOneShot(GetRandom(attackClips));

            weapon.DoAttack();
            return true;
        }
        else { return false; }
    }

    internal override void OnDeath(GameObject source)
    {
        base.OnDeath(source);
        int p = Random.Range(1, 20);

        if (p < 5 && bodyTemplate != null)
        {
            var body = Instantiate(bodyTemplate);
            body.transform.position = this.transform.position;
            body.SetActive(true);
        }
    }
}
