using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextIsland : MonoBehaviour
{
    public void NextIsland()
    {
        IslandNamer.SetNextRandomIslandName();
        LoadScript.LoadTo(Scenes.GameplayScene, "Loading...");
    }
}
