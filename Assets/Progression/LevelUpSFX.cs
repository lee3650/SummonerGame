using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSFX : MonoBehaviour
{
    [SerializeField] XPApplier XPApplier;
    [SerializeField] AudioClip LevelUpSound;
    [SerializeField] AudioSource Source;
    [SerializeField] AudioSource LevelUpSource;
    [SerializeField] AudioClip GainXPSound;

    private void Awake()
    {
        XPApplier.LeveledUp += LeveledUp;
        XPApplier.GainedXP += GainedXP;
    }

    private void GainedXP()
    {
        print("playing gain xp sound!");
        GameplaySFX.PlaySoundOnSource(GainXPSound, SharedSoundManager.GetSFXVolumeLevel(), Source);
    }

    private void LeveledUp()
    {
        GameplaySFX.PlaySoundOnSource(LevelUpSound, SharedSoundManager.GetSFXVolumeLevel(), LevelUpSource);
    }
}
