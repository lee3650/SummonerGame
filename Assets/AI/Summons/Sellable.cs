using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sellable : MonoBehaviour
{
    public float SellPrice; 
    public string GetSellText()
    {
        return "Sell Price: " + SellPrice; 
    }
}
