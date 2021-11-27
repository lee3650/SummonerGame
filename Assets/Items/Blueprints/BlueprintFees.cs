using System.Collections.Generic;
using UnityEngine;
using System;

public class BlueprintFees : MonoBehaviour, IResettable
{
    //The blueprint weapons will subscribe to this (in case price changes while selected).
    //this will know about the UI and force it to update
    //At the very least, we can initialize random prices

    //oh, this needs to get reset... well, no it doesn't but we could reset it. 
    public static event Action<BlueprintType, float> PriceUpdated = delegate { };

    [SerializeField] ItemInfoDisplayer ItemInfoDisplayer;
    [SerializeField] BlueprintPrice[] Prices;

    private static Dictionary<BlueprintType, float> TypeToPrice = new Dictionary<BlueprintType, float>();
    private static Dictionary<BlueprintType, BlueprintPrice> TypeToPriceObj = new Dictionary<BlueprintType, BlueprintPrice>();

    public void ResetState()
    {
        TypeToPrice = new Dictionary<BlueprintType, float>();
        TypeToPriceObj = new Dictionary<BlueprintType, BlueprintPrice>();
        PriceUpdated = null;
        PriceUpdated = delegate { };
    }

    public static float GetMinFee(BlueprintType type)
    {
        return TypeToPriceObj[type].minPrice;
    }

    public static float GetMaxFee(BlueprintType type)
    {
        return TypeToPriceObj[type].maxPrice;
    }

    public void InitializePrices() //this needs to happen after the home tile is placed, so that it doesn't mess up the tutorial map
    {
        if (LetterManager.UseGameplayChange(GameplayChange.RandomPrices))
        {
            foreach (BlueprintPrice p in Prices)
            {
                float startPrice = UnityEngine.Random.Range(p.minPrice, p.maxPrice);
                RoundAndSetPrice(p, startPrice);
            }
        }
        else
        {
            foreach (BlueprintPrice p in Prices)
            {
                float startPrice = (p.minPrice + p.maxPrice) / 2;
                RoundAndSetPrice(p, startPrice);
            }
        } 
    }

    private void RoundAndSetPrice(BlueprintPrice p, float startPrice)
    {
        startPrice = RoundToHundreds(startPrice);
        SetPrice(p, startPrice);
    }

    private float RoundToHundreds(float input)
    {
        int hundreds = (int)(input * 100);
        return (float)hundreds / 100f;
    }

    private void SetPrice(BlueprintPrice p, float startPrice)
    {
        p.startPrice = startPrice;
        
        TypeToPrice[p.Type] = startPrice;
        TypeToPriceObj[p.Type] = p;
    }

    private void Awake()
    {
        if (LetterManager.UseGameplayChange(GameplayChange.IncrementPrice)) //is this okay? 
        {
            BlueprintManager.BlueprintAdded += BlueprintAdded;
        }
    }

    private void BlueprintAdded(BlueprintType type)
    {
        TypeToPrice[type] = TypeToPrice[type] + TypeToPriceObj[type].Increment;
        PriceUpdated(type, TypeToPrice[type]);
        ItemInfoDisplayer.RefreshSelectedItemUI(type);
    }

    public void BlueprintRemoved(BlueprintType type)
    {
        TypeToPrice[type] = TypeToPrice[type] - TypeToPriceObj[type].Increment;
        BlueprintManager.SetFeesForType(type, TypeToPriceObj[type].startPrice, TypeToPriceObj[type].Increment);
        PriceUpdated(type, TypeToPrice[type]);
        ItemInfoDisplayer.RefreshSelectedItemUI(type);
    }

    public static float GetMaintenanceDelta(BlueprintType type)
    {
        return TypeToPriceObj[type].Increment;
    }

    public static float GetMaintenanceFee(BlueprintType type)
    {
        return TypeToPrice[type];
    }
}
