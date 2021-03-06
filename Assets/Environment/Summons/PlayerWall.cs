using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerWall : MonoBehaviour, ITargetable, IEntity, IInitialize
{
    [SerializeField] protected HealthManager HealthManager;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] protected Summon MySummon;
    [SerializeField] TileType TileType;
    [SerializeField] int Precedence;

    private ISubEntity[] SubEntities;

    protected MapNode prevNode;

    public virtual void Init()
    {
        print("Awake called parent!");

        MySummon.SummonerSet += SummonerSet;

        HealthManager.OnDeath += OnDeath;
        HealthManager.OnHealthChanged += OnHealthChanged;
        
        transform.position = VectorRounder.RoundVector(transform.position);

        prevNode = MapManager.ReadPoint(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        if (prevNode == null)
        {
            throw new System.Exception("Prev node was set to null!");
        }

        SubEntities = GetComponents<ISubEntity>();

        WriteMyTileToMap(); 
    }

    void SummonerSet()
    {
        ReplaceTileUnderneathIfThereIsOne();
        MySummon.SummonerSet -= SummonerSet; 
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
                        if (prevNode == null)
                        {
                            throw new System.Exception("Prev node was null!");
                        }
                    }
                    s.Destroy();

                    WriteMyTileToMap();
                }
            }
        }
    }

    public virtual bool ShouldBeOverwritten()
    {
        return true; 
    }

    public TileType GetTileType()
    {
        return TileType;
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

    public void RemoveSummon()
    {
        HealthManager.SubtractHealth(10000);
        gameObject.SetActive(false);
        print("Todo: remove these inactive gameobjects!");
    }

    private void OnHealthChanged()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, HealthManager.GetHealthPercentage());
    }

    protected virtual void OnDeath()
    {
        //I'm not going to destroy, I'm just going to disable everything - hopefully that reduces the number of null errors here 
        print("writing prev node: " + prevNode.TileType);
        MapManager.WritePoint(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), prevNode);
        HealthManager.OnDeath -= OnDeath;
        HealthManager.OnHealthChanged -= OnHealthChanged;
        gameObject.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public virtual void HandleEvent(Event e)
    {
        e = MySummon.GetSummoner().GetCharmModifiedEvent(e, MySummon.GetSummonType());

        foreach (ISubEntity s in SubEntities)
        {
            e = s.ModifyEvent(e);
        }

        foreach (ISubEntity s in SubEntities)
        {
            s.HandleEvent(e);
        }

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
        return Precedence;
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
