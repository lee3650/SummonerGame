using UnityEngine;
using System;

public class NextLevelEvent : MonoBehaviour
{
    public event Action OnNextLevel = delegate { };
    public void TriggerOnNextLevelEvent()
    {
        OnNextLevel();
    }
}
