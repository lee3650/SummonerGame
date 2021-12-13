using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMiner : PlayerWall, ILivingEntity, IWaveNotifier, IControllableSummon, IEarner, IRanged
{
    [SerializeField] float MoneyPerWave = 5f;
    [SerializeField] float Range = 15f;
    [SerializeField] bool AddToTargeting = true; 

    public override void Init()
    {
        print("Awake called!");
        if (AddToTargeting)
        {
            TargetableEntitiesManager.AddTargetable(this);
        }
        WaveSpawner.NotifyWhenWaveEnds(this);
        base.Init();
    }

    public float GetIncomePreview(Vector2 pos)
    {
        return GetMoneyMultipler(pos) * MoneyPerWave;
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
        float multiplier = GetMoneyMultipler(transform.position);
        MySummon.GetSummoner().AddMana(multiplier * MoneyPerWave);
    }

    float GetMoneyMultipler(Vector2 position)
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
            Vector2 pos = VectorRounder.RoundVector(position) + dirs[i];

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
        return GetMoneyMultipler(transform.position) * MoneyPerWave;
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

    public string GetStatString(Vector2 pos)
    {
        return string.Format("Money per wave: {0}", GetMoneyMultipler(pos) * MoneyPerWave);
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
