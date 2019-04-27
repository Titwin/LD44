using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimationController : MonoBehaviour
{
    private SpriteRenderer sr;
    private int animationIndex;
    private float animationTime;
    private AnimationType lastAnimation;

    public Weapon weapon;
    public float timeIdle;
    public Sprite[] animationIdle;

    public float timeWalking;
    public Sprite[] animationWalking;

    public float timeDucking;
    public Sprite[] animationDucking;

    public float timeInAir;
    public Sprite[] animationInAir;

    public float timeAttack;
    public Sprite[] animationAttack;

    public enum Weapon
    {
        SWORD,
        AXE,
        BOW,
        MAGIC
    }
    public enum AnimationType{
        IDLE,
        WALKING,
        DUCKING,
        INAIR,
        ATTACK
    }

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animationTime = 0.0f;
        animationIndex = 0;
    }

    // Update is called once per frame
    public void playAnimation(AnimationType animType, bool flipped = false)
    {
        Sprite[] animation;
        float t;

        switch (animType)
        {
            case AnimationType.WALKING:
                animation = animationWalking;
                t = timeWalking;
                break;
            case AnimationType.DUCKING:
                animation = animationDucking;
                t = timeDucking;
                break;
            case AnimationType.ATTACK:
                animation = animationAttack;
                t = timeAttack;
                break;
            case AnimationType.INAIR:
                animation = animationInAir;
                t = timeInAir;
                break;
            default:
                animation = animationIdle;
                t = timeIdle;
                break;
        }



        if (animation.Length == 0)
            return;

        if (animType != lastAnimation)
        {
            lastAnimation = animType;
            animationTime = 0.0f;
            animationIndex = 0;

            sr.sprite = animation[animationIndex];
            sr.flipX = flipped;
        }


        if (animationTime >= t)
        {
            animationTime -= t;
            animationIndex = (animationIndex + 1) % animation.Length;
            sr.sprite = animation[animationIndex];
            sr.flipX = flipped;
        }
        animationTime += Time.deltaTime;
    }
}
