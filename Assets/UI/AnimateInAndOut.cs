using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimateInAndOut : MonoBehaviour
{
    [SerializeField] Animator Animator;
    [SerializeField] AnimationClip AnimateIn;
    [SerializeField] AnimationClip AnimateOut;

    [SerializeField] bool InterruptAnimations = true;

    [SerializeField] bool Shown = true;
    [SerializeField] bool AnimateInOnStart = false;

    public event Action AnimatingOut = delegate { };

    private void Start()
    {
        if (AnimateInOnStart)
        {
            PlayAnimateIn();
        }    
    }

    public void ToggleVisibility()
    {
        if (Shown)
        {
            PlayAnimateOut();
        } else
        {
            PlayAnimateIn();
        }
    }

    public bool IsShown 
    { 
        get
        {
            return Shown;
        } 
    }

    private bool CanPlayAnimation()
    {
        return Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f || InterruptAnimations;
    }

    private void PlayAnimateOut()
    {
        if (CanPlayAnimation())
        {
            AnimatingOut();
            Animator.Play(AnimateOut.name);
            Shown = false;
            print("Animating out!");
        }
    }

    public void PlayAnimateIn()
    {
        if (CanPlayAnimation())
        {
            Animator.Play(AnimateIn.name, -1, 0f);
            Shown = true;
            print("Animating in!");
        }
    }
}
