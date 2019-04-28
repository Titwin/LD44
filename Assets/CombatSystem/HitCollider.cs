using UnityEngine;

public class HitCollider : TriggerCollider
{
    public Attackable character;

    protected override void Awake()
    {
        base.Awake();

        if (character == null)
        {
            Debug.LogError("character is null on HitBox on GameObject " + gameObject.name);
        }
    }
}
