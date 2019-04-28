using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public abstract class Character : Attackable
{
    public Weapon weapon;

    [SerializeField] GameObject bodyTemplate;
    [SerializeField] SpriteRenderer sprite;

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

        sprite = GetComponent<SpriteRenderer>();
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

    protected override void OnHurt(GameObject source)
    {
        base.OnHurt(source);
        StartCoroutine(DoBlink());
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

    bool blinking = false;
    IEnumerator DoBlink()
    {
        if (!blinking)
        {
            blinking = true;
            Color initialColor = sprite.color;
            Color currentColor = initialColor;
            float t = 0;
            while (health.Invulnerable)
            {
                currentColor.a = ((int)((Mathf.Cos(t * 360*5f)*0.5f+0.5f)*10))/10f;
                sprite.color = currentColor;
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }
            sprite.color = initialColor;
            blinking = false;
        }
    }
}
