using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour, IDamager, IWielder
{
    [SerializeField] Projectile Projectile;
    [SerializeField] float AttackLength;
    [SerializeField] float projDir; 
    float timer = 0f;

    List<Event> AttackModifiers = new List<Event>();

    private void Update()
    {
        if (!WaveSpawner.IsCurrentWaveDefeated) //I'm pretty sure I shouldn't put this in update. 
        {
            timer += Time.deltaTime;
            if (timer > AttackLength)
            {
                timer = 0f;
                Attack();
            }
        }
    }

    private void Attack()
    {
        Projectile p = Instantiate(Projectile, transform.position, Quaternion.Euler(new Vector3(0f, 0f, projDir)));
        p.Fire(this);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnHit(IEntity hit)
    {

    }

    public List<Event> ModifyEventList(List<Event> original)
    {
        //I'm pretty sure I should not modify original. 

        List<Event> result = new List<Event>();

        for (int i = 0; i < original.Count; i++)
        {
            result.Add(original[i]);
        }

        for (int i = 0; i < AttackModifiers.Count; i++)
        {
            result.Add(AttackModifiers[i]);
        }

        return result; 
    }

    public void AddAttackModifier(Event e)
    {
        AttackModifiers.Add(e);
    }
}
