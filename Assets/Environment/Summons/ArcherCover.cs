using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCover : MonoBehaviour, IEntity
{
    [SerializeField] SpriteRenderer sr;

    [SerializeField] float Health;

    float MaxHealth;

    private void Awake()
    {
        MaxHealth = Health; 
    }

    public void HandleEvent(Event e)
    {
        switch (e.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                TakeDamage(e.Magnitude);
                break; 
        }
    }
    
    void TakeDamage(float damage)
    {
        Health -= damage; 
        if (Health <= 0)
        {
            Destroy(gameObject);
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Health / MaxHealth);
    }
}
