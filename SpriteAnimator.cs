using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float fps;
    //public bool playOnStart;

    [SerializeField]
    private List<SpriteAnimation> animations;
    private SpriteAnimation activeAnimation;

    private Action actionOnFinishedPlaying;

    private bool shouldPlay;

    private float animationTime;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (false == shouldPlay)
        {
            return;
        }

        animationTime += Time.deltaTime * fps;

        if (AnimationType.Loop == activeAnimation.animationType)
        {
            spriteRenderer.sprite = activeAnimation.frames[(int)animationTime % activeAnimation.frames.Length];
        }
        else //Play once animation
        {
            if ((int)animationTime >= activeAnimation.frames.Length) //has reached end
            {
                if(FrameOnFinished.last == activeAnimation.frameOnFinished)
                {
                    spriteRenderer.sprite = activeAnimation.frames[activeAnimation.frames.Length - 1];
                }
                else
                {
                    spriteRenderer.sprite = activeAnimation.frames[0];
                }
                

                actionOnFinishedPlaying?.Invoke();

                shouldPlay = false;
            }
            else
            {
                spriteRenderer.sprite = activeAnimation.frames[(int)animationTime];
            }
        }
    }

    public void Play(string animationName, Action actionOnComplete = null)
    {
        activeAnimation = null;

        foreach (var animation in animations)
        {
            if (animationName == animation.animationName)
            {
                actionOnFinishedPlaying = actionOnComplete;
                SelectAnimation(animation);
                break;
            }
        }

        if(null == activeAnimation)
        {
            throw new System.ArgumentException($"No animation with name {animationName} exists.");
        }
    }

    public void Play(int animationIndex, Action actionOnComplete = null)
    {
        if(animationIndex < 0 || animationIndex >= animations.Count)
        {
            throw new System.IndexOutOfRangeException($"No animation with index {animationIndex} exists.");
        }

        actionOnFinishedPlaying = actionOnComplete;
        SelectAnimation(animations[animationIndex]);
    }

    public void Play()
    {
        if (null == activeAnimation)
        {
            throw new System.NullReferenceException("No animation selected");
        }

        shouldPlay = true;
    }

    public void Pause()
    {
        Play();

        shouldPlay = false;
    }

    public void Restart()
    {
        shouldPlay = true;
        animationTime = 0;

        Update();
    }

    public void Reset()
    {
        shouldPlay = false;
        animationTime = 0;

        spriteRenderer.sprite = GetDefaultSprite();
    }

    private void SelectAnimation(SpriteAnimation animation)
    {
        activeAnimation = animation;
        fps = animation.frames.Length / animation.animaitonTime;

        shouldPlay = true;
        animationTime = 0;
    }


    public Sprite GetDefaultSprite(string animationName)
    {
        foreach (var animation in animations)
        {
            if (animationName == animation.name)
            {               
                return GetDefaultSprite(animation);
            }
        }

        throw new System.ArgumentException($"No animation with name {animationName} exists.");
    }

    public Sprite GetDefaultSprite(int animationIndex)
    {
        if (animationIndex < 0 || animationIndex >= animations.Count)
        {
            throw new System.IndexOutOfRangeException($"No animation with index {animationIndex} exists.");
        }

        return GetDefaultSprite(animations[animationIndex]);
    }

    private Sprite GetDefaultSprite(SpriteAnimation animation)
    {
        return animation.frames[FrameOnFinished.first == animation.frameOnFinished ? 0 : ^1];
    }

    private Sprite GetDefaultSprite()
    {
        return activeAnimation.frames[FrameOnFinished.first == activeAnimation.frameOnFinished ? 0 : ^1];
    }   
}
