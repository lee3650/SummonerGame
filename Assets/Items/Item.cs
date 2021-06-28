using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour //base class for every item 
{
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Collider2D col;
    private bool AbleToBePickedUp = true;

    protected Transform Wielder; 

    public bool CanBePickedUp()
    {
        return AbleToBePickedUp;
    }

    public virtual Sprite GetInventorySprite()
    {
        return SpriteRenderer.sprite;
    }

    public virtual void OnPickup(Transform collector, Transform wielder)
    {
        AbleToBePickedUp = false;
        SpriteRenderer.enabled = false;
        transform.parent = collector;
        col.enabled = false;
        Wielder = wielder; 
    }

    public virtual void OnDrop()
    {
        AbleToBePickedUp = true;
        SpriteRenderer.enabled = true;
        transform.parent = null;
        col.enabled = true;
    }

    public virtual void OnSelection()
    {
        SpriteRenderer.enabled = true;
        col.enabled = true; 
    }
    
    public virtual void OnDeselection()
    {
        SpriteRenderer.enabled = false;
        col.enabled = false;
    }
}
