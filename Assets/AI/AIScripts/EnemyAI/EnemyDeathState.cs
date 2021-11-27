using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : AIDeathState
{
    [SerializeField] string DeathMessage;
    [SerializeField] float XPGain;

    public override void VirtualEnterState()
    {
        XPManager.AddXPMessage(new XPMessage(DeathMessage, XPGain));
        base.VirtualEnterState();
    }
}