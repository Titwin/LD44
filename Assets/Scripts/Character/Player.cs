using System.Collections.Generic;
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

    protected override void Awake()
    {
        base.Awake();

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
            health.Value += consumable.healthModifier;
        }

        Debug.Log(name + " has picked " + item.name + "(" + item.GetType() + ")");
        return true;
    }

    protected virtual void SetWeapon(Weapon weapon)
    {
        this.weapon.damages = weapon.damages;
        health.Value += weapon.healthModifier - this.weapon.healthModifier;
        health.Value = Mathf.Max(health.Value, 1);

        foreach(Weapon w in weapons)
        {
            w.gameObject.SetActive(w.type == weapon.type);
            if (w.type == weapon.type) this.weapon = w;
        }
        AnimationController.Weapon t;
        switch (weapon.type)
        {
            case Weapon.Type.SWORD: t = AnimationController.Weapon.SWORD; break;
            case Weapon.Type.AXE: t = AnimationController.Weapon.AXE; break;
            case Weapon.Type.BOW: t = AnimationController.Weapon.BOW; break;
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
}
