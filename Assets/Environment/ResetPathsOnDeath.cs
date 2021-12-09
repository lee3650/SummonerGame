using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPathsOnDeath : MonoBehaviour
{
    [SerializeField] HealthManager hm;

    private void Awake()
    {
        hm.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        PathManager.ResetPaths = true; 
    }
}
