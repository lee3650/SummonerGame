using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour //base class for every item 
{
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Collider2D col;
    private bool AbleToBePickedUp = true;

    public bool CanBePickedUp()
    {
        return AbleToBePickedUp;
    }

    public virtual Sprite GetInventorySprite()
    {
        return SpriteRenderer.sprite;
    }

    public virtual void OnPickup(Transform collector)
    {
        AbleToBePickedUp = false;
        SpriteRenderer.enabled = false;
        transform.parent = collector;
        col.enabled = false;
    }

    public virtual void OnDrop()
    {
        AbleToBePickedUp = true;
        SpriteRenderer.enabled = true;
        transform.parent = null;
        col.enabled = true;
    }
}
