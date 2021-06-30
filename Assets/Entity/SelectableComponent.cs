using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableComponent : MonoBehaviour
{
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Material SelectedMaterial;
    [SerializeField] GameObject SelectGraphic;

    GameObject actualSelectGraphic;

    private void Awake()
    {
        actualSelectGraphic = Instantiate<GameObject>(SelectGraphic, transform);
        actualSelectGraphic.transform.localScale *= 1.25f;
        actualSelectGraphic.transform.localPosition = Vector3.zero;
        SpriteRenderer r = actualSelectGraphic.AddComponent<SpriteRenderer>();
        r.sprite = SpriteRenderer.sprite;
        r.sortingOrder = -1;
        r.color = Color.white;
        r.material = SelectedMaterial;

        actualSelectGraphic.SetActive(false);
    }

    public void Select()
    {
        actualSelectGraphic.SetActive(true);
    }

    public void Deselect()
    {
        actualSelectGraphic.SetActive(false);
    }
}
