using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombstoneSpriteManager : MonoBehaviour
{
    [SerializeField] Sprite[] TombstoneSprites;

    static Sprite[] StaticSprites;

    private void Awake()
    {
        StaticSprites = TombstoneSprites;
    }
    
    public static Sprite GetRandomTombstone()
    {
        return StaticSprites[Random.Range(0, StaticSprites.Length)];
    }
}
