using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeVisualizer : MonoBehaviour, IInitialize
{
    [SerializeField] GameObject RangeGraphicPrefab;
    [SerializeField] GameObject SquareGraphicPrefab; 

    GameObject MyRangeGraphic; 

    IRanged myRanged; 

    void Awake()
    {
        if (TryGetComponent<IRanged>(out myRanged))
        {
            if (myRanged.IsCrossShaped() == false)
            {
                MyRangeGraphic = Instantiate(RangeGraphicPrefab, transform);
                MyRangeGraphic.transform.localPosition = Vector3.zero;
                MyRangeGraphic.transform.localScale = new Vector2(myRanged.GetRange() * 2, myRanged.GetRange() * 2); //so, just hopefully everything is 1x1 lol
                Show();
            } else
            {
                //obviously this is kind of gross
                MyRangeGraphic = Instantiate(RangeGraphicPrefab, transform);
                MyRangeGraphic.transform.localPosition = Vector3.zero;
                GameObject sqr1 = Instantiate(SquareGraphicPrefab, MyRangeGraphic.transform);
                sqr1.transform.localPosition = Vector3.zero;
                sqr1.transform.localScale = new Vector2(myRanged.GetRange() * 2, myRanged.GetCrossDelta() * 2);
                GameObject sqr2 = Instantiate(SquareGraphicPrefab, MyRangeGraphic.transform);
                sqr2.transform.localPosition = Vector3.zero;
                sqr2.transform.localScale = new Vector2(myRanged.GetCrossDelta() * 2, myRanged.GetRange() * 2);
                Show();
            }
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
            if (myRanged.IsCrossShaped())
            {
                MyRangeGraphic.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
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
