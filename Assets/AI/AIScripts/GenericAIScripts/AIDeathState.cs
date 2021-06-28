using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : MonoBehaviour, IState
{
    [SerializeField] MovementController MovementController;
    [SerializeField] Collider2D col;

    public void EnterState()
    {
        col.enabled = false;
        MovementController.DisableAllMovement();
    }
    public void UpdateState()
    {

    }
    public void ExitState()
    {

    }
}
