using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caltrop : MonoBehaviour
{
    [SerializeField] Projectile MyProjectile;

    public void Init(List<Event> eventsToApply)
    {
        transform.position += (Vector3)(Random.insideUnitCircle * 0.45f);
        transform.eulerAngles = new Vector3(0, 0, Random.Range(-180f, 180f));

        foreach (Event e in eventsToApply)
        {
            MyProjectile.AddAttackModifier(Event.ScaleEvent(e, 0.5f));
        }
    }
}
