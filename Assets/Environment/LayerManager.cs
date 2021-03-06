using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    static Dictionary<Factions, string> FactionToProjLayer = new Dictionary<Factions, string>(); 

    public void Awake()
    {
        FactionToProjLayer[Factions.Nonplayer] = "EnemyAttack";
        FactionToProjLayer[Factions.Player] = "PlayerAttack";
    }

    public static string GetProjLayerFromFaction(Factions factions)
    {
        return FactionToProjLayer[factions];
    } 
}

