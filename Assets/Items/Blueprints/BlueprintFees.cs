using UnityEngine;
using System;

public class BlueprintFees : MonoBehaviour
{
    //The blueprint weapons will subscribe to this (in case price changes while selected).
    //this will know about the UI and force it to update
    //At the very least, we can initialize random prices
    public static event Action<BlueprintType, float> PriceUpdated = delegate { }; 
            
    public void InitializePrices() //this needs to happen after the home tile is placed, so that it doesn't mess up the tutorial map
    {

    }

    private void Awake()
    {
        BlueprintManager.BlueprintsChanged += BlueprintsChanged;
    }

    private void BlueprintsChanged()
    {
        //refresh prices
    }

    //I want this all to be serialized so... 
    public static float GetMaintenanceFee(BlueprintType type)
    {
        throw new System.Exception("Not implemented!");
    }
}
