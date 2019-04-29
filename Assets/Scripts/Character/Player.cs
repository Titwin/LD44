﻿using System.Collections.Generic;
﻿using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : Character
{
    public CharacterController2D Controller { get; private set; }
    [SerializeField] Weapon[] weapons;
    private Dictionary<AnimationController.Weapon, AnimationController> animationControllers = new Dictionary<AnimationController.Weapon, AnimationController>();
    //private Dictionary<Weapon.Type, Weapon> weaponDictionary = new Dictionary<Weapon.Type, Weapon>();

    // comodity property for AI, can be replaced with line-of-sight player detection
    public static Player thePlayer;

    protected void Awake()
    {
        Controller = GetComponent<CharacterController2D>();
        thePlayer = this;

        AnimationController[] aclist = GetComponents<AnimationController>();
        foreach(AnimationController ac in aclist)
        {
            animationControllers[ac.weapon] = ac;
        }
        /*foreach(Weapon w in weapons)
        {
            weaponDictionary[w.type] = w;
        }*/
        SetWeapon(this.weapon);
    }

    public void reset()
    {
        
    }

    protected void Update()
    {
        //base.Update();

        // set the movement actions
        float movementX = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");
        bool duck = Input.GetButton("Duck");
        bool attack = Input.GetButtonDown("Fire1");
        Controller.Move(movementX, jump, attack, duck);

        if (Controller.canAttack && attack)
        {
            weapon.DoAttack();
        }
    }

    public bool TryPick(MonoBehaviour item)
    {
        // TODO: implement picking behaviour
        // - check pickableObject.pickableType
        // - if object picked:
        // --  do the action associated to picking
        // - return true in case the object was picked

        var weapon = item as Weapon;
        if (weapon != null)
        {
            SetWeapon(weapon);
        }

        var consumable = item as ConsumableItem;
        if (consumable != null)
        {
            health.Heal(item.gameObject, consumable.healthModifier);
        }

        Debug.Log(name + " has picked " + item.name + "(" + item.GetType() + ")");
        return true;
    }

    protected virtual void SetWeapon(Weapon weapon)
    {
        this.weapon.damages = weapon.damages;
        int delta = weapon.healthModifier - this.weapon.healthModifier;
        delta = Mathf.Min(health.Value - 1, delta);
        if (delta>0) health.Heal(weapon.gameObject,delta);
        else if (delta < 0) health.Hurt(weapon.gameObject, delta);

        foreach(Weapon w in weapons)
        {
            w.gameObject.SetActive(w.type == weapon.type);
            if (w.type == weapon.type) this.weapon = w;
        }
        AnimationController.Weapon t;
        switch (weapon.type)
        {
            case Weapon.Type.SWORD: t = AnimationController.Weapon.SWORD; break;
            case Weapon.Type.MAGIC: t = AnimationController.Weapon.MAGIC; break;

            default: t = AnimationController.Weapon.SWORD; break;
        }
        foreach(KeyValuePair<AnimationController.Weapon, AnimationController> ac in animationControllers)
        {
            if(ac.Key == t)
            {
                ac.Value.enabled = true;
            }
            else
            {
                ac.Value.enabled = false;
            }
        }
        Controller.ac = animationControllers[t];
    }

    protected override void OnDeath(GameObject source)
    {
        thePlayer = null;
        Controller.ac.playAnimation(AnimationController.AnimationType.DYING);
    }
}
