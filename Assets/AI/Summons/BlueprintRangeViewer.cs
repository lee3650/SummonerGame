using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintRangeViewer : MonoBehaviour, IRanged
{
    [SerializeField] GameObject RangeSupplier;
    [SerializeField] RangeVisualizer RangeVisualizer;
    IRanged supplier; 

    void Awake()
    {
        supplier = RangeSupplier.GetComponent<IRanged>();
        RangeVisualizer.CreateAndShowGraphic();
    }

    public float GetRange()
    {
        return supplier.GetRange();
    }

    public float GetCrossDelta()
    {
        return supplier.GetCrossDelta();
    }

    public bool IsCrossShaped()
    {
        return supplier.IsCrossShaped();
    }
}