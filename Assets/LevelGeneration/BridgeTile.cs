using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTile : MonoBehaviour
{
    [SerializeField] Sprite LandSprite;
    [SerializeField] private bool Up;
    [Tooltip("left to right or bottom to top")]
    [SerializeField] Sprite[] Sprites;
    [Tooltip("0 is positive, 1 is negative")]
    [SerializeField] GameObject[] cols;

    public void SetGraphic()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            hit.collider.enabled = false;
        }

        if (ShouldHide())
        {
            print("hiding!");
            GetComponent<SpriteRenderer>().sprite = LandSprite;
        } 
        else
        {
            Sprite s = CalculateSprite();
            GetComponent<SpriteRenderer>().sprite = s;
        }

        TryDisableColliders();
    }

    public void TryDisableColliders()
    {
        Vector2 offset = Up ? new Vector2(1, 0) : new Vector2(0, 1); //if it's up, then we check RIGHT, because the colliders are perpendicular 
        
        if (MapManager.IsPointTraversable((Vector2)transform.position + offset, true))
        {
            cols[0].SetActive(false);
        }

        if (MapManager.IsPointTraversable((Vector2)transform.position - offset, true))
        {
            cols[1].SetActive(false);
        }
    }

    private Sprite CalculateSprite()
    {
        Vector3 offset;
        
        if (Up)
        {
            offset = new Vector3(0, 1, 0);
        } 
        else
        {
            offset = new Vector3(1, 0, 0);
        }

        if (MapManager.GetTileType(transform.position + offset) == TileType.Bridge) //if in the positive offset
        {
            if (MapManager.GetTileType(transform.position - offset) == TileType.Bridge) //and the negative offset
            {
                return Sprites[1]; //center sprite
            }
            else
            {
                return Sprites[0]; //otherwise only the positive offset
            }
        }
        else
        {
            return Sprites[2]; //only the negative offset 
        }
    }

    private bool ShouldHide()
    {
        Vector2 offset1;
        Vector2 offset2;

        if (Up)
        {
            offset1 = new Vector2(0, 1);
            offset2 = new Vector2(0, -1);

        }
        else
        {
            offset1 = new Vector2(1, 0);
            offset2 = new Vector2(-1, 0);
        }

        Vector2 p1 = (Vector2)transform.position + offset1;
        Vector2 p2 = (Vector2)transform.position + offset2;

        if (MapManager.GetTileType(p1) != TileType.Bridge && MapManager.GetTileType(p2) != TileType.Bridge)
        {
            return true;
        }

        return false; 
    }
}
