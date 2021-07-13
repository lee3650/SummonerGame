using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //so, this should take instead a list of 'features' 
    public MapNode[,] GenerateLevel(Vector2 dimension, List<MapFeature> features)
    {
        //last step needs to be surrounding the map with walls - yeah this is kind of lame because 'dimension' has to be subtracted 1 from. 
    }
}
