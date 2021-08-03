using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerWall : MonoBehaviour, ITargetable, IEntity
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Summon MySummon; 
    [SerializeField] TileType TileType;

    private MapNode prevNode; 

    private void Awake()
    {
        MySummon.SummonerSet += ReplaceTileUnderneathIfThereIsOne;
        
        HealthManager.OnDeath += OnDeath;
        HealthManager.OnDamageTaken += OnDamageTaken;
        
        transform.position = VectorRounder.RoundVector(transform.position);

        prevNode = MapManager.ReadPoint(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        WriteMyTileToMap(); 
    }

    void ReplaceTileUnderneathIfThereIsOne()
    {
        for (int i = MySummon.GetSummoner().GetSummons().Count - 1; i >= 0; i--)
        {
            Summon s = MySummon.GetSummoner().GetSummons()[i];
            if (s.GetSummonType() == SummonType.Wall)
            {
                if (s.transform.position == transform.position && s != MySummon)
                {
                    print("Destroying summon!");
                    PlayerWall playerWall; 
                    if (s.TryGetComponent<PlayerWall>(out playerWall))
                    {
                        prevNode = playerWall.GetUnderneathNode();
                    }
                    s.Destroy();

                    WriteMyTileToMap();
                }
            }
        }
    }

    public bool CanBeTargetedBy(Factions faction)
    {
        if (faction != Factions.Player)
        {
            return true; 
        }
        return false; 
    }

    void WriteMyTileToMap()
    {
        MapManager.WritePoint(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), new MapNode(true, TileType));
    }

    public bool RequireLineOfSight()
    {
        return false;
    }
    public MapNode GetUnderneathNode()
    {
        return prevNode;
    }

    private void OnDamageTaken()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, HealthManager.GetHealthPercentage());
    }

    private void OnDeath()
    {
        //I'm not going to destroy, I'm just going to disable everything - hopefully that reduces the number of null errors here 
        print("writing prev node: " + prevNode.TileType);
        MapManager.WritePoint(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), prevNode);
        gameObject.SetActive(false);
    }

    public void HandleEvent(Event e)
    {
        switch (e.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                HealthManager.SubtractHealth(e.Magnitude);
                break; 
        }
    }

    public bool CanBeTargeted()
    {
        return IsAlive();
    }

    public int GetPrecedence()
    {
        return 1000; 
    }
    public Vector2 GetPosition()
    {
        return transform.position;
    }
    public bool IsAlive()
    {
        return HealthManager.IsAlive();
    }

    public bool IsDamaged()
    {
        return HealthManager.GetCurrent() < HealthManager.GetMax();
    }
}
