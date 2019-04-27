using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damages;
    public int healthModifier;
    public Type type;

    public enum Type
    {
        SWORD,
        AXE,
        BOW,
        MAGIC
    }
}
