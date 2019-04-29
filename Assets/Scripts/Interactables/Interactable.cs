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
            collision.gameObject.GetComponent<Player>().Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player.Interactable == this)
            {
                player.Interactable = null;
            }
        }
    }
}
