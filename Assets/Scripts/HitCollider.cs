using UnityEngine;

public class HitCollider : TriggerCollider
{
    public Character character;

    protected override void Awake()
    {
        base.Awake();

        if (character == null)
        {
            Debug.LogWarning("character is null on HitBox on GameObject " + gameObject.name);
        }
    }
}
