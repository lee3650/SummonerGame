using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeVisualizer : MonoBehaviour, IInitialize
{
    [SerializeField] GameObject RangeGraphicPrefab;

    GameObject MyRangeGraphic; 

    IRanged myRanged; 

    void Awake()
    {
        if (TryGetComponent<IRanged>(out myRanged))
        {
            MyRangeGraphic = Instantiate(RangeGraphicPrefab, transform);
            MyRangeGraphic.transform.localPosition = Vector3.zero;
            MyRangeGraphic.transform.localScale = new Vector2(myRanged.GetRange() * 2, myRanged.GetRange() * 2); //so, just hopefully everything is 1x1 lol
            Show();
        }
    }

    public void Init()
    {
        Hide(); //this could be interesting lol. This could get nasty. 
    }

    public void Show()
    {
        if (MyRangeGraphic != null)
        {
            MyRangeGraphic.SetActive(true);
        }
    }

    public void Hide()
    {
        if (MyRangeGraphic != null)
        {
            MyRangeGraphic.SetActive(false);
        }
    }
}
