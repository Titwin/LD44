using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    bool blinking = false;
    // huge hack
    public new bool canFade = true;
    public new bool canBlink = true;

    public void Fade(float time)
    {
        if(canFade)
            StartCoroutine(DoFade(time));
    }
    public void Blink(float time)
    {
        if(canBlink)
            StartCoroutine(DoBlink(time));
    }
        
    IEnumerator DoFade(float time)
    {
        if (!blinking)
        {
            Color initialColor = sprite.color;
            Color targetColor = new Color(0, 0, 0, 0);
            float t = 0;
            while (t < time)
            {
                sprite.color = Color.Lerp(initialColor, targetColor, t / time);
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }
            sprite.color = targetColor;
        }
    }
    IEnumerator DoBlink(float time)
    {
        if (!blinking)
        {
            blinking = true;
            Color initialColor = sprite.color;
            Color currentColor = initialColor;
            float t = 0;
            while (blinking && t < time)
            {
                currentColor.a = ((int)((Mathf.Cos(t * 360 * 5f) * 0.5f + 0.5f) * 10)) / 10f;
                sprite.color = currentColor;
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }
            sprite.color = initialColor;
            blinking = false;
        }
    }
}
