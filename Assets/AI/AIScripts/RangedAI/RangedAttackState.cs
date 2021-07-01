using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AIAttackState
{
    //so, we need to serialize a projectile here. That's fine. 
    //I recall the collision code being super complicated. Hm.
    
    [SerializeField] Projectile Projectile;
    [SerializeField] Transform firingPosition;

    public override void StartAttack()
    {
        Projectile p = Instantiate(Projectile, firingPosition.position, transform.rotation);
        p.Fire();
    }
}
