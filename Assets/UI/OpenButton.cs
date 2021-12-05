using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] GifDisplayer GifDisplayer;
    [SerializeField] ImageSequence Glimmer;
    [SerializeField] ImageSequence Open;
    [SerializeField] Sprite Normal;
    [SerializeField] RewardPanel RewardPanel;

    private bool interactable = true;

    public bool Interactable
    {
        get
        {
            return interactable;
        }
        set
        {
            interactable = value;
            if (value == true)
            {
                GifDisplayer.Stop();
                GifDisplayer.SetSprite(Normal);
            } else
            {
                StopAllCoroutines();
            }
        }
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (Interactable)
        {
            GifDisplayer.PlayGifOneShot(Open);
            StartCoroutine(GiveRewardWhenFinished());
        }
    }

    IEnumerator GiveRewardWhenFinished()
    {
        while (!GifDisplayer.Finished())
        {
            yield return null;
        }
        RewardPanel.OpenButtonPressed();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        print("pointer entered!");

        print("gifdisplayer finished: " + GifDisplayer.Finished());

        if (GifDisplayer.Finished() && Interactable)
        {
            GifDisplayer.PlayGif(Glimmer);
        }
    }
}
