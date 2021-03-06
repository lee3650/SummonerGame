using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorRounder : MonoBehaviour
{
    public static Vector2 RoundVector(Vector2 input)
    {
        return new Vector2(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y));
    }

    public static Vector2Int RoundVectorToInt(Vector2 input)
    {
        return new Vector2Int(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y));
    }
}
