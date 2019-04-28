using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Character owner;

    public int damages;
    public int healthModifier;
    public float cooldown;
    public Type type;

    float currentCooldown;
    public enum Type
    {
        SWORD,
        AXE,
        BOW,
        MAGIC
    }

    /*
    public bool showTriggerDebug;
    
    protected virtual void Update()
    {
        if (showTriggerDebug)
        {
            var inRangeCharacterNames = attackCollider.InRange.Select(character => character.name).ToArray();
            Debug.Log(gameObject.name + " is triggering [" + string.Join(", ", inRangeCharacterNames) + "]");
        }
    }*/

    private void Update()
    {
        currentCooldown -= Time.deltaTime;
        currentCooldown = Mathf.Max(0, currentCooldown);
    }
    public virtual bool CanAttack()
    {
        return currentCooldown == 0;
    }
    public virtual void DoAttack()
    {
        currentCooldown = cooldown;
    }
    public abstract bool InRange(Attackable target);
}
