using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarMouseoverEnabler : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AnimateInAndOut AnimateInAndOut;
    
    public void OnPointerEnter(PointerEventData data)
    {
        if (!AnimateInAndOut.IsShown)
        {
            AnimateInAndOut.ToggleVisibility();
        }
    }
}
