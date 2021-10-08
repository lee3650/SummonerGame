using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTurner : PlayerWall
{
    [SerializeField] float DirectionAdjustment = -90f;
    [SerializeField] DirectionalAnimator Animator;
    private float zRot;

    private void Awake()
    {
        zRot = transform.eulerAngles.z;
        transform.eulerAngles = Vector3.zero;
        Animator.IdleDirection(zRot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile p;
        if (collision.TryGetComponent<Projectile>(out p))
        {
            p.transform.position = transform.position;
            p.Rotate(zRot + DirectionAdjustment);
        }
    }

    public override void HandleEvent(Event e)
    {
        //do nothing. I don't want these to be able to be hit
    }
}
