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

    private int UnlockedLevel;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    public void SetGraphic(Sprite s, DisplayRewardData rd, int unlockedLevel)
    {
        image.sprite = s;
        drd = rd;
        UnlockedLevel = unlockedLevel;
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
            if (drd != null)
            {
                if (ExperienceManager.GetCurrentLevel() < UnlockedLevel)
                {
                    if (drd.IsItem)
                    {
                        panelInstance = Instantiate(UnlockedRewardPanel, PanelSpawnPos);
                        panelInstance.ShowPreview(drd);
                    }
                } else
                {
                    panelInstance = Instantiate(UnlockedRewardPanel, PanelSpawnPos);
                    panelInstance.ShowPreview(drd);
                }
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
