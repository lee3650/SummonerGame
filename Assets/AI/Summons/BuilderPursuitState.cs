using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderPursuitState : SummonPursuitState
{
    protected override bool ShouldKeepPursuingTarget()
    {
        return TargetManager.Target.IsDamaged();
    }
}
