using System.Linq;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Health health;

    public Weapon weapon;

    public AttackCollider attackCollider;

    public bool showTriggerDebug;

    protected virtual void Awake()
    {
        if (health == null)
        {
            Debug.LogError(name + " has no health");
        }

        if (weapon == null)
        {
            Debug.LogError(name + " has no weapon");
        }
    }

    protected virtual void Update()
    {
        if (showTriggerDebug)
        {
            var inRangeCharacterNames = attackCollider.InRange.Select(character => character.name).ToArray();
            Debug.Log(gameObject.name + " is triggering [" + string.Join(", ", inRangeCharacterNames) + "]");
        }
    }

    public virtual void Attack()
    {
        foreach (var character in attackCollider.InRange)
        {
            character.health.Value -= weapon.damages;
        }
    }
}
