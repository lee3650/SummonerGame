using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour //base class for every item 
{
    [SerializeField] SpriteRenderer SpriteRenderer;
    private bool AbleToBePickedUp = true;

    public bool CanBePickedUp()
    {
        return AbleToBePickedUp;
    }

    public virtual void OnPickup(Transform collector)
    {
        AbleToBePickedUp = false;
        SpriteRenderer.enabled = false; 
    }
    public virtual void OnDrop()
    {
        AbleToBePickedUp = true; 
    }
}
