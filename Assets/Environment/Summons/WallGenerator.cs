using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

//I named everything wall related but technically this could apply to any summon 
public class WallGenerator : PlayerWall, ILivingEntity
{
    [SerializeField] float Range = 25;
    [SerializeField] int MaxNumSummons = 6;

    [SerializeField] List<BlueprintType> Types; 
    [SerializeField] List<GameObject> Prefabs;

    List<HealthManager> SummonedEntities = new List<HealthManager>();

    protected override void Awake()
    {
        TargetableEntitiesManager.AddTargetable(this);
        MySummon.SummonerSet += SummonerSet;
        MySummon.SummonWaveEnds += WaveEnds;
        base.Awake();
    }

    private void WaveEnds()
    {
        PlaceSummonsWhereAble();
    }

    private void SummonerSet()
    {
        PlaceSummonsWhereAble();
        BlueprintManager.BlueprintsChanged += BlueprintsChanged;
        MySummon.SummonerSet -= SummonerSet;
    }

    private void BlueprintsChanged()
    {
        if (WaveSpawner.IsCurrentWaveDefeated)
        {
            PlaceSummonsWhereAble();
        }
    }

    private void PlaceSummonsWhereAble()
    {
        PruneSummonedEntitiesList();

        int wallsToPlace = MaxNumSummons - SummonedEntities.Count;
        PlaceSummons(Types, Prefabs, wallsToPlace);
    }

    private void PlaceSummons(List<BlueprintType> types, List<GameObject> prefabs, int numWalls)
    {
        List<Blueprint> prints = BlueprintManager.GetBlueprintsOfTypes(types);

        //I feel like this knows a bit too much - it could rely better on abstraction 

        for (int i = 0; i < numWalls; i++)
        {
            foreach (Blueprint p in prints)
            {
                if (!p.Satisfied)
                {
                    int index = types.IndexOf(p.BlueprintType);

                    p.Satisfied = true;

                    GameObject summoned = SummonEntity(prefabs[index], p.Point); 

                    HealthManager hm = summoned.GetComponent<HealthManager>();
                    SummonedEntities.Add(hm);
                    hm.OnDeath += PruneSummonedEntitiesList; //okay that's a little better. 
                }
            }
        }
    }

    protected virtual GameObject SummonEntity(GameObject entity, Vector2 endPoint)
    {
        return SummonWeapon.SpawnSummon(entity, endPoint, MySummon.GetSummoner(), Quaternion.Euler(Vector3.zero));
    }

    private void PruneSummonedEntitiesList()
    {
        for (int i = SummonedEntities.Count - 1; i >= 0; i--)
        {
            if (SummonedEntities[i].IsAlive() == false)
            {
                //I also need to reset satisfied. Hm. 
                SetSatisfied(SummonedEntities[i], false);
                SummonedEntities[i].OnDeath -= PruneSummonedEntitiesList;
                SummonedEntities.RemoveAt(i);
            }
        }
    }

    protected virtual void SetSatisfied(HealthManager entity, bool val)
    {
        BlueprintManager.SetSatisfied(entity.transform.position, val);
    }

    protected override void OnDeath()
    {
        TargetableEntitiesManager.RemoveTargetable(this);
        BlueprintManager.BlueprintsChanged -= BlueprintsChanged;
        MySummon.SummonWaveEnds -= WaveEnds;
        base.OnDeath();
    }

    public Factions GetFaction()
    {
        return Factions.Player;
    }

    public void OnHit(IEntity hit)
    {
        throw new System.Exception("The wall generator got a hit?");
    }

    public List<Event> ModifyEventList(List<Event> unmodifiedList)
    {
        throw new System.Exception("Should not ask the wall generator to modify a list of events!");
    }
}
