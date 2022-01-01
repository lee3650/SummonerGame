using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMinerCharm : MonoBehaviour, ISubCharmHandler
{
    [SerializeField] PlayerMiner PlayerMiner;

    public void ApplyCharm(Charm c)
    {
        switch (c)
        {
            case CharmOfEfficiency ce:
                PlayerMiner.MultiplyBaseMoneyPerWave(ce.EfficiencyModifier);
                break; 
        }
    }
}
