using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseoverGrey : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("If null, assigned at awake to GetComponent")]
    [SerializeField] Image image;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
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
