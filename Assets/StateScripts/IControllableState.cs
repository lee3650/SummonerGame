using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllableState : IState
{
    void HandleCommand(PlayerCommand command);
}