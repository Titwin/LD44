﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableTrigger : Attackable
{
    [SerializeField] bool value = false;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ActivatedObject activatedObject;
    public float timer;
    private float t;
    
    private void Start()
    {
        Set(value);
    }

    protected override void OnAttack(GameObject source)
    {
        base.OnAttack(source);
        Set(!value);
    }
    void Set(bool _value)
    {
        this.value = _value;
        if (!value)
        {
            DoDisable();
        }
        else
        {
            t = timer;
            DoEnable();
        }

        if (activatedObject != null)
            activatedObject.SetActive(value);
    }

    private void Update()
    {
        if (t - Time.deltaTime < 0)
            Set(false);
        if(t > 0)
            t -= Time.deltaTime;
    }

    virtual protected void DoDisable()
    {
        sprite.color = Color.gray;
    }
    virtual protected void DoEnable()
    {
        sprite.color = Color.magenta;
    }
}
