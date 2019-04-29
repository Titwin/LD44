using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAltar : Interactable
{
    static bool toggledGlobal = true;
    [SerializeField] bool toggled = false; 
    public Weapon item1;
    public Weapon item2;

    MonoBehaviour CurrentItem
    {
        get {
            if (toggled)
                return item2;
            else return item1;
        }
    }
    
    private void Start()
    {
        UpdateState();
    }
    private void Update()
    {
        if(toggled!=toggledGlobal)
        {
            toggled = toggledGlobal;
            UpdateState();
        }
    }
    public override bool CanInteract(Character character)
    {
        return character.gameObject.tag == "Player" && CurrentItem.gameObject.activeSelf;
    }

    public override bool DoInteract(Character character)
    {
        if (CanInteract(character))
        {
            MonoBehaviour item = toggled? item2:item1;
            bool picked = ((Player)character).TryPick(item);
            if (picked)
            {
                toggledGlobal = !toggled;
                UpdateState();
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

    void UpdateState()
    {
        item1.gameObject.SetActive(!toggled);
        item2.gameObject.SetActive(toggled);

    }

}
