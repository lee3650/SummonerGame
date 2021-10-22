using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class HotbarSelectorScript : MonoBehaviour, IPointerClickHandler
{
    public event Action HotbarSelectorClicked = delegate { };

    public void OnPointerClick(PointerEventData data)
    {
        HotbarSelectorClicked();
    }
}
