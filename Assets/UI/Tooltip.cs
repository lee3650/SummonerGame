using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] RectTransform TooltipTransform;
    [SerializeField] Vector2Int offset; 

    public void OnPointerEnter(PointerEventData data)
    {
        TooltipTransform.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        TooltipTransform.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        TooltipTransform.gameObject.SetActive(false);
    }
}
