using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMiner : PlayerWall, ILivingEntity, IWaveNotifier, IControllableSummon, IEarner, IRanged
{
    [SerializeField] float MoneyPerWave = 5f;
    [SerializeField] float Range = 15f;

    public override void Init()
    {
        print("Awake called!");
        TargetableEntitiesManager.AddTargetable(this);
        WaveSpawner.NotifyWhenWaveEnds(this);
        base.Init();
    }

    public override bool ShouldBeOverwritten()
    {
        return false; 
    }

    public List<Event> ModifyEventList(List<Event> umodifiedList)
    {
        return umodifiedList;
    }

    public void OnWaveEnds()
    {
        print("Got wave ends notification!");

        float multiplier = GetMoneyMultipler();
        MySummon.GetSummoner().AddMana(multiplier * MoneyPerWave);
    }

    float GetMoneyMultipler()
    {
        float multiplier = 0f;

        Vector2[] dirs = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
        };

        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = VectorRounder.RoundVector(transform.position) + dirs[i];

            if (MapManager.IsTileType((int)pos.x, (int)pos.y, TileType.Gold))
            {
                multiplier += 2;
            }
            else if (MapManager.IsTileType((int)pos.x, (int)pos.y, TileType.Silver))
            {
                multiplier += 1.5f;
            }
            else if (MapManager.IsTileType((int)pos.x, (int)pos.y, TileType.Copper))
            {
                multiplier += 1;
            }
        }

        return multiplier;
    }

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

    public float GetIncome()
    {
        return GetMoneyMultipler() * MoneyPerWave;
    }

    public float GetRange()
    {
        return Range;
    }

    public bool IsCrossShaped()
    {
        return false; 
    }

    public float GetCrossDelta()
    {
        return 0f;
    }

    public Factions GetFaction()
    {
        return Factions.Player;
    }

    public void HandleCommand(PlayerCommand command)
    {
        switch (command)
        {
            case SellCommand sc:
                RemoveSummon();
                BlueprintManager.ForceBlueprintsChanged(); //so, this is contingent on the blueprint satisfiers relying on miners... hm. 
                break;
            case UpgradeCommand uc:
                RemoveSummon();
                SummonWeapon.UpgradeSummon(uc.UpgradePath, transform.position, MySummon.GetSummoner(), GetComponent<Sellable>());
                break; 
        }
    }

    public bool CanBeSelected()
    {
        return HealthManager.IsAlive(); 
    }

    public string GetStatString()
    {
        return string.Format("Money per wave: {0}", GetMoneyMultipler() * MoneyPerWave);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnHit(IEntity hit)
    {
        throw new System.Exception("The miner got a hit?");
    }

    public bool CanBeSold()
    {
        return AdjacentConnections.DoAdjacentTilesConnectToMiner(new Vector2Int((int)transform.position.x, (int)transform.position.y));
    }

    protected override void OnDeath()
    {
        WaveSpawner.StopNotifyingWhenWaveEnds(this);
        TargetableEntitiesManager.RemoveTargetable(this);
        base.OnDeath();
    }
}
