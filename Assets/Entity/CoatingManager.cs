using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoatingManager : MonoBehaviour
{
    private Coating CurrentCoating;

    float graphicTimer = 0f;
    const float minTime = 0.1f;

    public Event ModifyEvent(Event input)
    {
        if (CurrentCoating != null)
        {
            if (graphicTimer > minTime)
            {
                graphicTimer = 0f;
                CritGraphicPool.ShowCrit((Vector2)transform.position + new Vector2(0, 1));
            }
            return CurrentCoating.GetModifiedEvent(input);
        } else
        {
            return input; 
        }
    }

    public float GetMoveSpeedAdjustment() 
    {
        if (CurrentCoating == null)
        {
            return 1f; 
        }
        return CurrentCoating.GetMoveSpeedAdjustment();
    }

    public List<Event> ModifyAttackEvents(List<Event> unmodifiedList)
    {
        if (CurrentCoating == null)
        {
            return unmodifiedList;
        }
        return CurrentCoating.ModifyAttackEvents(unmodifiedList);
    }

    public virtual void SetCoating(Coating newCoating)
    {
        CurrentCoating = newCoating;
    }

    private void Update()
    {
        if (CurrentCoating != null)
        {
            CurrentCoating.TimeLeft -= Time.deltaTime;
            if (CurrentCoating.TimeLeft <= 0f)
            {
                CurrentCoating = null; 
            }
        }

        graphicTimer += Time.deltaTime;
    }
}
