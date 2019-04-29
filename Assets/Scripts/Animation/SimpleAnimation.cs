using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
    private SpriteRenderer sr;
    private int animationIndex;
    private float animationTime;
    public bool randomInitialization = false;
    public bool flipX = false;
    public bool flipY = false;
    public float perFrameTime;
    public Sprite[] animations;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(randomInitialization)
        {
            animationTime = Random.Range(0, perFrameTime);
            animationIndex = Random.Range(0, animations.Length - 1);
        }
        else
        {
            animationTime = 0.0f;
            animationIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!sr) return;
        if (animations.Length == 0)
            return;

        if (animationTime >= perFrameTime)
        {
            animationTime -= perFrameTime;
            animationIndex = (animationIndex + 1) % animations.Length;
            sr.sprite = animations[animationIndex];
            sr.flipX = flipX;
            sr.flipY = flipY;
        }
        animationTime += Time.deltaTime;
    }
}
