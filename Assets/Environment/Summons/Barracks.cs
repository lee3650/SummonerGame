using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : PlayerWall, ILivingEntity, IWaveNotifier, IControllableSummon
{
    [SerializeField] GameObject Summon;
    [SerializeField] PointToHoldManager PointToHoldManager;

    [SerializeField] string StatString; 

    ILivingEntity lastSummon; 
    
    //maybe each barracks can hold two guys? We'll have to see. That might be a charm thing too, if we're doing that. 

    protected override void Awake()
    {
        print("Awake called!");
        TargetableEntitiesManager.AddTargetable(this);
        WaveSpawner.NotifyWhenWaveEnds(this);
        PointToHoldManager.PointToHold = new Vector2(transform.position.x, transform.position.y) + new Vector2(1, 0);
        MySummon.SummonerSet += SummonerSet;
        base.Awake();
    }

    private void SummonerSet()
    {
        CreateSummon();
        MySummon.SummonerSet -= SummonerSet;
    }

    public void OnWaveEnds()
    {
        //so, we're going to summon a guy if we don't already have one. 

        if (lastSummon.IsAlive() == false)
        {
            CreateSummon();
        }
    }

    public void HandleCommand(PlayerCommand command)
    {
        switch (command)
        {
            case HoldPointCommand hp:
                PointToHoldManager.PointToHold = hp.PointToHold;
                break;
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetStatString()
    {
        return StatString;
    }

    public bool CanBeSelected()
    {
        return true; 
    }

    void CreateSummon()
    {
        GameObject s = SummonWeapon.SpawnSummon(Summon, PointToHoldManager.PointToHold, MySummon.GetSummoner(), Quaternion.Euler(Vector2.zero));
        lastSummon = s.GetComponent<ILivingEntity>();
    }

    public override bool ShouldBeOverwritten()
    {
        return false;
    }

    public List<Event> ModifyEventList(List<Event> umodifiedList)
    {
        return umodifiedList;
    }

    public Factions GetFaction()
    {
        return Factions.Player;
    }

    public void OnHit(IEntity hit)
    {

    }

    protected override void OnDeath()
    {
        TargetableEntitiesManager.RemoveTargetable(this);
        WaveSpawner.StopNotifyingWhenWaveEnds(this);
        base.OnDeath();
    }
}
