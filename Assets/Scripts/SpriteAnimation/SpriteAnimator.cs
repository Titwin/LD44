using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Animations")]
    [SerializeField] SpriteAnimation currentAnimation;

    [SerializeField] SpriteAnimation[] animations;
    Dictionary<int, SpriteAnimation> animationDictionary = new Dictionary<int, SpriteAnimation>();

    [Header("Parameters")]
    [SerializeField] bool playOnStart = true;
    [SerializeField] float animationSpeed = 1;

    // Delegate callbacks
    public delegate void AnimatorCallback(SpriteAnimation current);
    public AnimatorCallback OnAnimationStart;
    public AnimatorCallback OnAnimationLoop;
    public AnimatorCallback OnAnimationEnd;

    #region state variables
    private float totalTime = 0;
    private float localTime = 0;
    private float frameRate;
    private int currentFrame = 0;
    private int loops = 0;
    private bool playing;
    private bool over = false;
    private bool firstFrame = true;
    #endregion

    private void Start()
    {
        // index the animations based on name for easy access
        foreach (SpriteAnimation animation in animations)
        {
            animationDictionary[GetAnimationHash(animation)] = animation;
        }
        // reset the animation
        Reset();
        playing = playOnStart;
    }
    // Update is called once per frame
    void Update()
    {
        if (playing && !over)
        {
            Tick();
        }
    }

    #region playback functionality

    public bool IsPlaying()
    {
        return playing;
    }
    public bool IsOver()
    {
        return over;
    }
    public void Play()
    {
        playing = true;
    }
    public void Pause()
    {
        playing = false;
    }
    public void Reset()
    {
        loops = 0;
        firstFrame = true;
        over = false;
        currentFrame = 0;
        totalTime = 0;
        localTime = 0;

        spriteRenderer.sprite = currentAnimation.frames[0];
        if (currentAnimation.animationDuration > 0)
        {
            frameRate = currentAnimation.frames.Length / currentAnimation.animationDuration;
        }
        else
        {
            frameRate = 0;
        }
    }

    public void PlayAnimation(SpriteAnimation animation, float timeOffset = 0)
    {
        this.currentAnimation = animation;
        Reset();
        localTime = timeOffset;
        playing = true;
    }
    void Tick()
    {
        int frame = currentFrame;
        if (frameRate > 0)
        {
            float deltaTime = Time.deltaTime * animationSpeed * frameRate;
            totalTime += deltaTime;
            localTime += deltaTime;
            frame = (int)localTime;
        }
        if (firstFrame || frame != currentFrame)
        {
            if (firstFrame)
            {
                firstFrame = false;

                // animator callback
                if (OnAnimationStart != null)
                {
                    OnAnimationStart(currentAnimation);
                }
            }
            if (frame >= currentAnimation.frames.Length)
            {
                if (currentAnimation.loop)
                {
                    ++loops;
                    frame = 0;
                    localTime = 0;

                    // animator callback
                    if (OnAnimationLoop != null)
                    {
                        OnAnimationLoop(currentAnimation);
                    }
                }
                else
                {
                    frame = currentAnimation.frames.Length - 1;
                    over = true;

                    // animator callback
                    if (OnAnimationEnd != null)
                    {
                        OnAnimationEnd(currentAnimation);
                    }
                }
            }
            currentFrame = frame;

            spriteRenderer.sprite = currentAnimation.frames[currentFrame];
        }
    }
    #endregion
    #region indexed animations
    public bool HasAnimationIndexed(string animationName)
    {
        return HasAnimationIndexed(GetAnimationHash(animationName));
    }

    public bool HasAnimationIndexed(int animationNameHash)
    {
        return animationDictionary.ContainsKey(animationNameHash);
    }
    public bool PlayAnimationIndexed(string animationName, float timeOffset = 0)
    {
        return PlayAnimationIndexed(GetAnimationHash(animationName), timeOffset);
    }
    public bool PlayAnimationIndexed(int animationNameHash, float timeOffset = 0)
    {
        if (HasAnimationIndexed(animationNameHash))
        {
            PlayAnimation(animationDictionary[animationNameHash], timeOffset);
            return true;
        }
        else
            return false;
    }
    public static int GetAnimationHash(SpriteAnimation animation)
    {
        return GetAnimationHash(animation.name);
    }
    public static int GetAnimationHash(string animationName)
    {
        return Animator.StringToHash(animationName);
    }
    #endregion
}
