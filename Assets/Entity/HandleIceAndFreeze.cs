using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleIceAndFreeze : MonoBehaviour, ISubEntity, ISpeedSupplier
{
    [SerializeField] SpriteRenderer sr;
    private float MoveSpeedAdjustment = 1f;

    float resetSpeedTimer = 0f;

    const float IceSpeed = 0.5f;
    const float FreezeSpeed = 0f;

    public float GetMoveSpeedAdjustment()
    {
        return MoveSpeedAdjustment;
    }

    public bool IsFrozen()
    {
        return resetSpeedTimer >= 0;
    }

    public void HandleEvent(Event e)
    {
        if (e.MyType == EventType.IceDamage)
        {
            if (MoveSpeedAdjustment > IceSpeed)
            {
                MoveSpeedAdjustment = IceSpeed;
            }
            resetSpeedTimer = e.Magnitude;
        }
        if (e.MyType == EventType.Freeze)
        {
            if (MoveSpeedAdjustment > FreezeSpeed)
            {
                MoveSpeedAdjustment = FreezeSpeed;
            }
            resetSpeedTimer = e.Magnitude;
        }
    }

    public Event ModifyEvent(Event e)
    {
        return e;
    }

    private void Update()
    {
        if (resetSpeedTimer > 0)
        {
            resetSpeedTimer -= Time.deltaTime;
            sr.color = new Color(0.5f, 0.5f, 1f);
            if (resetSpeedTimer <= 0)
            {
                sr.color = Color.white;
                MoveSpeedAdjustment = 1f;
            }
        }
    }
}
