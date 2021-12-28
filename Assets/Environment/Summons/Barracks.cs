using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : PlayerWall, ILivingEntity, IWaveNotifier, IControllableSummon
{
    [SerializeField] GameObject Summon;
    [SerializeField] PointToHoldManager PointToHoldManager;
    [SerializeField] BlueprintBarracks BlueprintBarracks;

    [SerializeField] List<string> StatString; 

    ILivingEntity lastSummon; 
    
    public override void Init()
    {
        print("Initialized a Barracks script! Name: " + name);
        TargetableEntitiesManager.AddTargetable(this);
        WaveSpawner.NotifyWhenWaveEnds(this);
        PointToHoldManager.PointToHold = new Vector2(transform.position.x, transform.position.y) + new Vector2(1, 0);
        MySummon.SummonerSet += SummonerSet;
        base.Init();
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

    //This is kind of sketchy - it's a lot of exactly duplicated, copy-paste code 
    public int SummonTier
    {
        get
        {
            return MySummon.SummonTier;
        }
    }

    public SummonType GetSummonType()
    {
        return MySummon.GetSummonType();
    }

    public string GetStatString(Vector2 pos)
    {
        string result = "";

        foreach (string s in StatString)
        {
            result += s + "\n";
        }
        return result;
    }

    public bool CanBeSelected()
    {
        return HealthManager.IsAlive();
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

    public bool CanBeSold()
    {
        Vector2Int pos = VectorRounder.RoundVectorToInt(transform.position);
        return AdjacentConnections.DoAdjacentTilesConnectToMiner(new List<Vector2Int> { pos, pos + BlueprintBarracks.GetSpawnOffset() });
    }


    protected override void OnDeath()
    {
        TargetableEntitiesManager.RemoveTargetable(this);
        WaveSpawner.StopNotifyingWhenWaveEnds(this);
        base.OnDeath();
    }
}
