using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float transitionDuration = 0.25f;
    bool show = false;
    Color currentColor;


    private void Update()
    {
        // TODO: use coroutines instead 
        currentColor = sprite.color;
        currentColor.a = Mathf.MoveTowards(currentColor.a, show ? 1 : 0, Time.deltaTime / transitionDuration);
        sprite.color = currentColor;
    }
    void Show()
    {
        show = true;
    }

    void Hide()
    {
        show = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Show();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Hide();
        }
    }
}
