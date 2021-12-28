using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFactory : ArrowTrap
{
    private List<Event> lastProjectileList; 

    protected override void DoAttack()
    {
        Projectile p = Instantiate(Projectile, SpawnPos.position, Quaternion.Euler(Vector3.zero));
        p.Fire(this, this);
        Caltrop c = p.GetComponent<Caltrop>();
        if (lastProjectileList != null)
        {
            c.Init(lastProjectileList);
        }
        lastProjectileList = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile p;
        if (collision.TryGetComponent<Projectile>(out p))
        {
            if (collision.TryGetComponent<Caltrop>(out Caltrop c))
            {
                return;
            }
            lastProjectileList = p.GetEventsToApply();
            DoAttack();
            if (Random.Range(0, 100) <= 50f)
            {
                Destroy(p.gameObject);
            }
        }
    }
}
