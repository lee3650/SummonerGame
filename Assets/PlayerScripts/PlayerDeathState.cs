using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : MonoBehaviour, IState
{
    [SerializeField] MovementController MovementController;
    public void EnterState()
    {
        MovementController.SetVelocity(Vector2.zero, 0f);
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {

    }
}
