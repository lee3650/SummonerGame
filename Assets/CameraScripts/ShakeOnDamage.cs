using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeOnDamage : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] Screenshake Screenshake;

    private void Start()
    {
        HealthManager.OnDamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken()
    {
        Screenshake.StartShake();
    }
}
