using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pickable : MonoBehaviour
{
    public MonoBehaviour item;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(this.name + " triggered by " + other.name);

        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            bool picked = player.TryPick(item);
            if (picked)
            {
                Destroy();
            }
        }
    }

    protected virtual void Destroy()
    {
        Destroy(this.gameObject);
    }
}
