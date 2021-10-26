using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class exposes a list of gameobjects associated with indexes - the index is the level at which that item is unlocked
public class ItemProgressionManager : MonoBehaviour
{
    //[SerializeField] List<ListGameObjectField> SetupItemProgression; 
    //private static List<ListGameObjectField> ItemProgression;

    //private void Awake()
    //{
    //    ItemProgression = SetupItemProgression;
    //}

    //public static List<GameObject> GetItemsUnlockedAtLevel(int level)
    //{
    //    if (level >= 0 && level < ItemProgression.Count)
    //    {
    //        return ItemProgression[level].GameObjects; //I guess the nice thing about this is it can't be null now
    //    }

    //    return new List<GameObject>();
    //}
}