using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashOnHit : MonoBehaviour
{
    [SerializeField] SquashAndStretch SquashAndStretch;
    [SerializeField] HealthManager hm;

    private void Awake()
    {
        hm.OnDamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken()
    {
        SquashAndStretch.StartSquash(false);
    }
}
