using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : MonoBehaviour, IState
{
    [SerializeField] MovementController MovementController;
    [SerializeField] Collider2D col;
    [SerializeField] SpriteRenderer SpriteRenderer;

    public void EnterState()
    {
        col.enabled = false;
        MovementController.DisableAllMovement();
    
        if (SpriteRenderer == null)
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 0.25f);
    }
    
    public void UpdateState()
    {

    }

    public void ExitState()
    {

    }
}
