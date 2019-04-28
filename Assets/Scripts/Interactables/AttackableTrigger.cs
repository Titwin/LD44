using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableTrigger : Attackable
{
    [SerializeField] bool value = false;
    [SerializeField] SpriteRenderer sprite;

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
            DoEnable();
        }
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
