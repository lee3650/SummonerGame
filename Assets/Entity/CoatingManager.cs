using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoatingManager : MonoBehaviour
{
    private Coating CurrentCoating; 
    
    public Event ModifyEvent(Event input)
    {
        if (CurrentCoating != null)
        {
            return CurrentCoating.GetModifiedEvent(input);
        } else
        {
            return input; 
        }
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
    }
}
