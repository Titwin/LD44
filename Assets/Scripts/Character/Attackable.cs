using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable: MonoBehaviour
{
    public Health health;

    [SerializeField] SpriteEffect spriteEffect;

    [SerializeField] float destroyTime = 0.1f;

    public AudioSource audioSource;

    public List<AudioClip> hurtClips;
    public AudioClip deathClip;

    public void DoDamage(GameObject source, int amount)
    {
        OnAttack(source!=null?source.gameObject:null);
        if (health != null)
        {
            Debug.Log(this.name + " got attacked for " + amount + "dmg" + ", out of " + health.Value);
            if (!health.Invulnerable)
            {
                health.Hurt(source,amount);

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
        Destroy(this.gameObject);
    }

    protected virtual void OnAttack(GameObject source)
    {
    }

    internal virtual void OnHurt(GameObject source)
    {
        spriteEffect.Blink(health.invulnerabilityTime);
        audioSource.PlayOneShot(GetRandom(hurtClips));
    }

    internal virtual void OnDeath(GameObject source)
    {
        spriteEffect.Blink(destroyTime);
        audioSource.PlayOneShot(deathClip);
    }

    protected virtual AudioClip GetRandom(List<AudioClip> audioClips)
    {
        if (audioClips.Count == 0)
        {
            Debug.LogWarning(name + " has mising audio clips!");
            return null;
        }

        int index = Mathf.RoundToInt(Random.value * (audioClips.Count - 1));
        return audioClips[index];
    }
}
