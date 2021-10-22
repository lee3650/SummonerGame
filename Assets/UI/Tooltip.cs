using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Tooltip : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] RectTransform TooltipTransform;
    [SerializeField] Vector2Int offset;
    [SerializeField] bool SendEvent = false;

    public event Action MousedOver = delegate { };

    public void OnPointerEnter(PointerEventData data)
    {
        TooltipTransform.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        TooltipTransform.gameObject.SetActive(false);
        if (SendEvent)
        {
            MousedOver();
        }
    }

    private void OnDisable()
    {
        TooltipTransform.gameObject.SetActive(false);
    }
}
