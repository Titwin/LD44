using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBody : Interactable
{
    [SerializeField] SpriteEffect effect;
    [SerializeField] int health = 1;
    //bool enabled = true;

    public override bool CanInteract(Character character)
    {
        return true;
    }

    public override bool DoInteract(Character character)
    {
        character.health.Value += health;
        //enabled = false;
        StartCoroutine(DoSlowDestroy(0.5f));
        return true;
    }

    IEnumerator DoSlowDestroy(float time)
    {
        effect.Fade(time);
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
