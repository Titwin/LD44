using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAnimator : MonoBehaviour
{
    private SpriteRenderer sr;

    public float timePerSprite;
    public Sprite[] animation;

    private int animationIndex;
    private float animationTime;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animationTime = 0.0f;
        animationIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (animation.Length == 0)
            return;
        
        if (animationTime >= timePerSprite)
        {
            animationTime -= timePerSprite;
            animationIndex = (animationIndex + 1) % animation.Length;
            sr.sprite = animation[animationIndex];
        }
        animationTime += Time.deltaTime;
    }
}
