using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warship : MonoBehaviour
{
    [SerializeField] float oceanSpeed;
    [SerializeField] float coastalSpeed;
    [SerializeField] float coastalTransitionDist;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite LandedSprite;
    private Vector2 startPoint;
    private const int xDelta = 2;

    public void GoToEndPoint()
    {
        startPoint = transform.position;
        StartCoroutine(TravelToEndPoint());
    }
    
    IEnumerator TravelToEndPoint()
    {
        Vector2 trueGoal = GetTrueGoal();

        float sum = 0f;

        while (Vector2.Distance(trueGoal, transform.position) > coastalTransitionDist)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            sum += oceanSpeed * Time.fixedDeltaTime;
            transform.position = Vector2.Lerp(startPoint, trueGoal, sum);
        }

        while (Vector2.Distance(trueGoal, transform.position) > 0.01f)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            transform.position = Vector2.Lerp(transform.position, trueGoal, coastalSpeed * Time.fixedDeltaTime);
        }

        sr.sprite = LandedSprite;
        transform.position = trueGoal;
    }

    public void SnapToDestination()
    {
        StopAllCoroutines();
        transform.position = GetTrueGoal();
        sr.sprite = LandedSprite;
    }

    private Vector2 GetTrueGoal()
    {
        return new Vector2(xDelta, 0) + Endpoint;
    }

    public Vector2 Endpoint
    {
        get;
        set;
    }
}
