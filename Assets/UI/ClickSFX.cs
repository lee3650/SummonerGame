using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSFX : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] bool OverrideDownSound;
    [SerializeField] AudioClip OverrideDown;
    [SerializeField] bool OverrideUpSound;
    [SerializeField] AudioClip OverrideUp;
    [SerializeField] bool PlayOnPointerUp = true; //if we really wanted to go crazy we could make that a new component
    [SerializeField] AudioSource AudioSource;

    public void OnPointerDown(PointerEventData data)
    {
        print("playing");

        AudioClip clipToPlay = SharedSoundManager.GetDefaultMouseDownSound();

        if (OverrideDownSound)
        {
            clipToPlay = OverrideDown;
        }

        PlaySound(clipToPlay);
    }

    public void OnPointerUp(PointerEventData data)
    {
        print("playing up");

        if (!PlayOnPointerUp)
        {
            return;
        }

        AudioClip clipToPlay = SharedSoundManager.GetDefaultMouseUpSound();

        if (OverrideUpSound)
        {
            clipToPlay = OverrideUp;
        }

        PlaySound(clipToPlay);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            print("clip was null!");
        }

        AudioSource.clip = clip;
        AudioSource.Play();
    }
}
