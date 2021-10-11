using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSummon : PlayerMiner
{
    public void SetHealthManager(HealthManager hm)
    {
        HealthManager = hm;
        MySummon.SetHealthManager(hm);
    }

    public override void HandleEvent(Event e)
    {
        if (e.Sender != null)
        {
            e.Sender.HandleEvent(e); //if it's itself it's going to cause some issues. 
        }
        base.HandleEvent(e);
    }
}
