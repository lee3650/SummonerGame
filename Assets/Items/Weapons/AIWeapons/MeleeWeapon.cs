using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] Animator Animator;
    [SerializeField] AnimationClip Attack;
    [SerializeField] float Magnitude;
    [SerializeField] EventType EventType;

    public void StartAttack()
    {
        Animator.Play(Attack.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEntity entity; 
        if (collision.TryGetComponent<IEntity>(out entity))
        {
            HandleCollision(entity);
        }
    }

    protected virtual void HandleCollision(IEntity entity)
    {
        entity.HandleEvent(new Event(EventType, Magnitude));
    }
}
