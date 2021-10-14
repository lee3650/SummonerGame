using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarMouseoverEnabler : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AnimateInAndOut AnimateInAndOut;
    [SerializeField] ItemSelection Hotbar;

    public void OnPointerEnter(PointerEventData data)
    {
        Hotbar.DeselectItem();
        AnimateInAndOut.ToggleVisibility();
    }
}
