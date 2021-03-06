using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class AIEntity : MonoBehaviour, ILivingEntity, IInitialize
{
    [SerializeField] Factions Faction;
    [SerializeField] int Precedence;
    [SerializeField] HealthManager HealthManager;
    [SerializeField] AIDeathState AIDeathState;
    [SerializeField] StateController StateController;
    [SerializeField] CoatingManager CoatingManager;
    [SerializeField] AIFallState AIFallState;

    [SerializeField] bool Targetable = true;

    ISubEntity[] SubEntities;

    public void Init()
    {
        if (Targetable)
        {
            TargetableEntitiesManager.AddTargetable(this);
        }
        if (CoatingManager == null)
        {
            CoatingManager = GetComponent<CoatingManager>();
        }

        SubEntities = GetComponents<ISubEntity>();
    }

    private void Start()
    {
        HealthManager.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        //enter death state
        StateController.TransitionToState(AIDeathState);
        if (Faction != Factions.Player)
        {
            EnemyDeathManager.OnEnemyDeath();
        }
    }

    public void WakeUp()
    {
        AISleepState aISleepState;
        if (TryGetComponent<AISleepState>(out aISleepState))
        {
            InitializeScripts();
            aISleepState.WakeUp();
        }
    }

    void InitializeScripts()
    {
        IInitialize[] initializes = GetComponents<IInitialize>();
        foreach (IInitialize i in initializes)
        {
            i.Init();
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool CanBeTargetedBy(Factions faction)
    {
        if (faction != Faction)
        {
            return true; 
        }
        return false; 
    }

    public bool RequireLineOfSight()
    {
        return true;
    }

    public virtual void OnHit(IEntity hit)
    {

    }

    public virtual List<Event> ModifyEventList(List<Event> unmodifiedList)
    {
        return CoatingManager.ModifyAttackEvents(unmodifiedList);
    }

    public virtual void HandleEvent(Event e)
    {
        e = CoatingManager.ModifyEvent(e);

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
            case EventType.Poison:
            case EventType.Freeze:
            case EventType.IceDamage: 
                HealthManager.SubtractHealth(e.Magnitude);
                break;
            case EventType.Fall:
                if (!(StateController.GetCurrentState() is AIFallState) && !(StateController.GetCurrentState() is AIDeathState))
                {
                    StateController.TransitionToState(AIFallState);
                }
                break;
            case EventType.Heal:
                CritGraphicPool.ShowHeal(transform.position + new Vector3(0, 1, 0));
                HealthManager.Heal(e.Magnitude);
                break;
            case EventType.DrainBlood:
                if (e.Sender != null)
                {
                    e.Sender.HandleEvent(new Event(EventType.Heal, e.Magnitude, e.Sender)); //this? 
                }
                HealthManager.SubtractHealth(e.Magnitude);
                break;
        }
    }
    
    public void OnDestroy()
    {
        TargetableEntitiesManager.RemoveTargetable(this);
    }

    public bool CanBeTargeted()
    {
        return IsAlive() && (StateController.GetCurrentState() is AISleepState == false); //so, only if we're awake, basically... 
    }

    public bool IsAlive()
    {
        return HealthManager.IsAlive();
    }

    public Factions GetFaction()
    {
        return Faction;
    }

    public int GetPrecedence()
    {
        return Precedence;
    }

    public bool IsDamaged()
    {
        return HealthManager.GetCurrent() < HealthManager.GetMax();
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
