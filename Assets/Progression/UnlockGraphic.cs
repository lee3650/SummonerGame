using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnlockGraphic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] Image image;
    [SerializeField] UnlockedRewardPanel UnlockedRewardPanel; //that's a prefab
    [SerializeField] Transform PanelSpawnPos;

    private DisplayRewardData drdInstance;
    private UnlockedRewardPanel panelInstance;

    public void SetGraphic(Sprite s, DisplayRewardData rd)
    {
        //mm. I guess better if this doesn't know about the progression system? 
        image.sprite = s;
        drdInstance = Instantiate(rd);
        drdInstance.TextPath = ""; //this isn't really ideal either... 
        //we should make a showPreview thing.
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (panelInstance.gameObject.activeInHierarchy)
        {
            if (panelInstance != null)
            {
                panelInstance.gameObject.SetActive(false);
            }
        }
        else
        {
            if (drdInstance.HasGif)
            {
                panelInstance = Instantiate(UnlockedRewardPanel, PanelSpawnPos);
                panelInstance.Show(drdInstance);
            }
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        image.color = new Color(0.75f, 0.75f, 0.75f, 1);
    }

    public void OnPointerExit(PointerEventData data)
    {
        image.color = new Color(1f, 1f, 1f, 1);
    }
}
