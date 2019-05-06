using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public abstract class Character : Attackable
{
    public Weapon weapon;

    [SerializeField] GameObject bodyTemplate;

    public List<AudioClip> attackClips;
    public CharacterController2D Controller { get; private set; }

    protected virtual void Awake()
    {
        Controller = GetComponent<CharacterController2D>();
    }
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

   /* internal override void OnHurt(GameObject source)
    {
        Vector3 d = (this.transform.position - source.transform.position);
        //d.y =1;
        d = d.normalized * 10;
        this.Controller.Push(d);
        Debug.DrawLine(this.transform.position, this.transform.position + d);
    }*/
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
