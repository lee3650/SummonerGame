using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] TravelToPointState TravelToPointState;
    [SerializeField] StateController StateController;
    [SerializeField] TargetSearcher TargetSearcher;

    private Summoner MySummoner;
    public float ManaRefundAmount;

    public void SetSummoner(Summoner summoner)
    {
        MySummoner = summoner;
    }

    public Vector2 GetSummonerPosition()
    {
        return MySummoner.GetPosition();
    }

    public virtual void GoToPoint(Vector2 point)
    {
        TravelToPointState.PointToTravelTo = point;
        StateController.TransitionToState(TravelToPointState);
    }

    public virtual void SetTarget(ITargetable target)
    {
        TargetSearcher.AssignTarget(target);
    }

    public void Awake()
    {
        if (HealthManager != null)
        {
            HealthManager.OnDeath += SummonEnds;
        }
    }

    protected virtual void SummonEnds()
    {
        MySummoner.OnSummonDeath(ManaRefundAmount);
    }
}
