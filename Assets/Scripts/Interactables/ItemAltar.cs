using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAltar : Interactable
{
    public MonoBehaviour item;

    public override bool CanInteract(Character character)
    {
        return character.gameObject.tag == "Player" && item.gameObject.activeSelf;
    }

    public override bool DoInteract(Character character)
    {
        if (CanInteract(character))
        {
            bool picked = ((Player)character).TryPick(item);
            if (picked)
            {
                item.gameObject.SetActive(false);
                //Destroy(item.gameObject);
                //item = null;

            }
            return true;
        }
        else
        {
            return false;
        }
    }

}
