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

    private DisplayRewardData drd;
    private UnlockedRewardPanel panelInstance;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    public void SetGraphic(Sprite s, DisplayRewardData rd)
    {
        image.sprite = s;
        drd = rd;
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (panelInstance != null)
        {
            if (panelInstance.gameObject.activeInHierarchy)
            {
                panelInstance.gameObject.SetActive(false);
            } else
            {
                panelInstance.gameObject.SetActive(true);
            }
        }
        else
        {
            if (drd != null && drd.HasGif)
            {
                panelInstance = Instantiate(UnlockedRewardPanel, PanelSpawnPos);
                panelInstance.ShowPreview(drd);
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
