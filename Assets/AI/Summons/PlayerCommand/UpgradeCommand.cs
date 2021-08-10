using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCommand : PlayerCommand
{
    public UpgradePath UpgradePath;

    public UpgradeCommand(UpgradePath path)
    {
        UpgradePath = path;
    }
}
