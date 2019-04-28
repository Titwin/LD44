using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable: MonoBehaviour
{
    public Health health;
    [SerializeField] SpriteRenderer sprite;

    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
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
        StartCoroutine(DoBlink());
    }

    protected virtual void OnDeath(GameObject source)
    {
        StopCoroutine(DoBlink());
        StartCoroutine(DoFade());
    }

    bool blinking = false;
    IEnumerator DoFade()
    {
        if (!blinking)
        {
            Color initialColor = sprite.color;
            Color targetColor = new Color(0, 0, 0, 0);
            float t = 0;
            while(t<destroyTime)
            {
                sprite.color = Color.Lerp(initialColor,targetColor,t/destroyTime);
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }
            sprite.color = targetColor;
        }
    }
    IEnumerator DoBlink()
    {
        if (!blinking)
        {
            blinking = true;
            Color initialColor = sprite.color;
            Color currentColor = initialColor;
            float t = 0;
            while (blinking && health.Invulnerable)
            {
                currentColor.a = ((int)((Mathf.Cos(t * 360 * 5f) * 0.5f + 0.5f) * 10)) / 10f;
                sprite.color = currentColor;
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }
            sprite.color = initialColor;
            blinking = false;
        }
    }
}
