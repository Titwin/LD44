using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract bool CanInteract(Character character);
    [SerializeField] public float interactionDuration = 1;
    // return true if interaction was performed
    public abstract bool DoInteract(Character character);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Controller.EnteredInteractable(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Controller.ExitedInteractable(this);
        }
    }
}
