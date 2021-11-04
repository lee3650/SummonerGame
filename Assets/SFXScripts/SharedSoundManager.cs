using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedSoundManager : MonoBehaviour
{
    [SerializeField] AudioClip DefaultMouseDownSound;
    [SerializeField] AudioClip DefaultMouseUpSound;

    private static AudioClip MDSound = null;
    private static AudioClip MUSound = null;

    void Start()
    {
        MDSound = DefaultMouseDownSound;
        MUSound = DefaultMouseUpSound;
    }

    public static AudioClip GetDefaultMouseDownSound()
    {
        if (MDSound == null)
        {
            MDSound = (AudioClip)Resources.Load("Audio/DefaultMouseDown"); //should not do it like this lol
        }

        return MDSound;
    }

    public static AudioClip GetDefaultMouseUpSound()
    {
        if (MUSound == null)
        {
            MUSound = Resources.Load<AudioClip>("Audio/DefaultMouseUp");
        }

        return MUSound;
    }
}
