using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntity : MonoBehaviour, ILivingEntity
{
    [SerializeField] Factions Faction;
    [SerializeField] int Precedence;
    [SerializeField] HealthManager HealthManager;
    [SerializeField] AIDeathState AIDeathState;
    [SerializeField] StateController StateController;

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
            aISleepState.WakeUp();
        }
    }
    
    public virtual void OnHit(IEntity hit)
    {

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

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
