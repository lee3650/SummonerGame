using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : PlayerWall
{
    [SerializeField] int Spreads;
    [SerializeField] float Damage;
    [SerializeField] int Recurrences;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile p;
        if (collision.TryGetComponent<Projectile>(out p))
        {
            p.AddAttackModifier(new Event(EventType.Fire, Damage, null, Recurrences, Spreads));
        }
    }
}
