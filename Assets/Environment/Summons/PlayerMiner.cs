using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMiner : PlayerWall, ILivingEntity, IWaveNotifier
{
    [SerializeField] float MoneyPerWave = 5f; 

    protected override void Awake()
    {
        print("Awake called!");
        TargetableEntitiesManager.AddTargetable(this);
        WaveSpawner.NotifyWhenWaveEnds(this);
        base.Awake();
    }

    public override bool ShouldBeOverwritten()
    {
        return false; 
    }

    public void OnWaveEnds()
    {
        print("Got wave ends notification!");

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

        MySummon.GetSummoner().AddMana(multiplier * MoneyPerWave);
    }

    public Factions GetFaction()
    {
        return Factions.Player;
    }

    public void OnHit(IEntity hit)
    {
        throw new System.Exception("The miner got a hit?");
    }

    protected override void OnDeath()
    {
        WaveSpawner.StopNotifyingWhenWaveEnds(this);
        TargetableEntitiesManager.RemoveTargetable(this);
        base.OnDeath();
    }
}
