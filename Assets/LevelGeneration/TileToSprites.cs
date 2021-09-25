using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileToSprites
{
    public TileType TileType;
    public Sprite[] Sprites;
    public bool UseDefaultObject = true;
    public GameObject OverridenPrefab; 
}