using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTurner : PlayerWall
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile p;
        if (collision.TryGetComponent<Projectile>(out p))
        {
            p.transform.position = transform.position;
            p.Rotate(-90f);
        }
    }
}
