using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMusketman : MonoBehaviour, ISubCharmHandler
{
    [SerializeField] RangedAttackState RangedAttackState;

    public void ApplyCharm(Charm c)
    {
        switch (c)
        {
            case Musketman m:
                RangedAttackState.SetProjectile(m.Projectile);
                break;
        }
    }
}
