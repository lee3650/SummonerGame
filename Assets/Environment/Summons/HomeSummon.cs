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
}
