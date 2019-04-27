using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Character : MonoBehaviour, IAttackable
{
    public AttackCollider attackCollider;

    public bool showTriggerDebug;

    public Health Health { get; private set; }

    public void DoDamage(int amount)
    {
        throw new System.NotImplementedException();
    }

    public int GetLayer()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void Awake()
    {
        Health = GetComponent<Health>();
    }

    protected virtual void Update()
    {
       /* if (showTriggerDebug)
        {
            var inRangeCharacterNames = attackCollider.InRange.Select(character => character.name).ToArray();
            Debug.Log(gameObject.name + " is triggering [" + string.Join(", ", inRangeCharacterNames) + "]");
        }*/
    }
}
