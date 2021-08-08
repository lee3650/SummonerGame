using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetCommand : PlayerCommand
{
    public SetTargetCommand(ITargetable target)
    {
        Target = target; 
    }

    public ITargetable Target
    {
        get;
        set;
    }
}
