using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateInAndOut : MonoBehaviour
{
    [SerializeField] Animator Animator;
    [SerializeField] AnimationClip AnimateIn;
    [SerializeField] AnimationClip AnimateOut;

    [SerializeField] bool Shown = true;

    public void ToggleVisibility()
    {
        if (Shown)
        {
            PlayAnimateOut();
            Shown = false;
        } else
        {
            PlayAnimateIn();
            Shown = true; 
        }
    }

    public bool IsShown 
    { 
        get
        {
            return Shown;
        } 
    }

    private void PlayAnimateOut()
    {
        Animator.Play(AnimateOut.name);
    }
    private void PlayAnimateIn()
    {
        Animator.Play(AnimateIn.name);
    }
}
