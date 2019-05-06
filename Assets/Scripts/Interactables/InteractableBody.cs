using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBody : Interactable
{
    //[SerializeField] SpriteEffect effect;
    [SerializeField] int health = 1;
    //bool enabled = true;
    public float perFrameTime;
    public Sprite[] animations;
    private SpriteRenderer sr;

    public override bool CanInteract(Character character)
    {
        return true;
    }

    public override bool DoInteract(Character character)
    {
        character.health.Heal(this.gameObject,health);
        //enabled = false;
        StartCoroutine(DoSlowDestroy(0.5f));
        return true;
    }

    IEnumerator DoSlowDestroy(float time)
    {
        //effect.Fade(time);
        foreach(Sprite s in animations)
        {
            GetComponent<SpriteRenderer>().sprite = s;
            yield return new WaitForSeconds(perFrameTime);
        }
        Destroy(this.gameObject);
    }
}
