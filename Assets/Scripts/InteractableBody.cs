using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBody : Interactable
{
    [SerializeField] int health = 1;
    public override bool CanInteract(Character character)
    {
        return true;
    }

    public override bool DoInteract(Character character)
    {
        character.health.Value += health;
        Destroy(this.gameObject);
        return true;
    }
}
