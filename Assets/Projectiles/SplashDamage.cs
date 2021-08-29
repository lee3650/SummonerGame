using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : Projectile
{
    [SerializeField] float SplashRadius = 4f;
    [SerializeField] LayerMask LayerMask;

    protected override void OnHit()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, SplashRadius, LayerMask);

        foreach (Collider2D col in cols)
        {
            IEntity e; 
            if (col.TryGetComponent<IEntity>(out e))
            {
                foreach (Event ev in EventsToApply)
                {
                    e.HandleEvent(ev);
                }
            }
        }

    }
}
