using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class AIEntity : MonoBehaviour, ILivingEntity, ISpeedSupplier, IInitialize
{
    [SerializeField] Factions Faction;
    [SerializeField] int Precedence;
    [SerializeField] HealthManager HealthManager;
    [SerializeField] AIDeathState AIDeathState;
    [SerializeField] StateController StateController;
    [SerializeField] CoatingManager CoatingManager;
    [SerializeField] AIFallState AIFallState;

    [SerializeField] bool Targetable = true; 

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

    public float GetMoveSpeedAdjustment()
    {
        return CoatingManager.GetMoveSpeedAdjustment();
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

        switch (e.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                HealthManager.SubtractHealth(e.Magnitude);
                break;
            case EventType.Fall:
                if (!(StateController.GetCurrentState() is AIFallState) && !(StateController.GetCurrentState() is AIDeathState))
                {
                    StateController.TransitionToState(AIFallState);
                }
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
