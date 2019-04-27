using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickableObject : MonoBehaviour
{

    public enum PickableType
    {
        blood, weapon_sword, weapon_axe, weapon_bow, weapon_magic, weapon_shield

    }
    [SerializeField] public PickableType objectType;

    void LateUpdate()
    {
        triggered = false;
    }
    bool triggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.name +" triggered by "+collision.name);
        
        if (collision.gameObject.tag == "Player")
        {
            if (!triggered)
            {
                triggered = true;
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    bool success = player.OnPickObject(this);
                    if (success)
                    {
                        Destroy();
                    }
                }
            }
        }
    }
    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
