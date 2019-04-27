using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Character : MonoBehaviour
{
    public AttackCollider attackCollider;

    public bool showTriggerDebug;

    public Health Health { get; private set; }

    protected virtual void Awake()
    {
        Health = GetComponent<Health>();
    }

    protected virtual void Update()
    {
        if (showTriggerDebug)
        {
            var inRangeCharacterNames = attackCollider.InRange.Select(character => character.name).ToArray();
            Debug.Log(gameObject.name + " is triggering [" + string.Join(", ", inRangeCharacterNames) + "]");
        }
    }
}
