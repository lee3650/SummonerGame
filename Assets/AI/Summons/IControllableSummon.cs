using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllableSummon
{
    void HandleCommand(PlayerCommand command);

    string GetStatString(Vector2 pos);

    bool CanBeSelected();

    SummonType GetSummonType();

    int SummonTier
    {
        get;
    }

    bool CanBeSold();

    Transform GetTransform();
}
