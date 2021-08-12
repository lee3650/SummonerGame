using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCoating : MonoBehaviour
{
    [SerializeField] CoatingType CoatingType;
    [SerializeField] float CoatingDuration; 
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        CoatingManager coatingManager;
        if (collision.TryGetComponent<CoatingManager>(out coatingManager) && ColliderOnSameTile(collision))
        {
            coatingManager.SetCoating(Coating.GetCoating(CoatingType, CoatingDuration));
        }
    }

    private bool ColliderOnSameTile(Collider2D col)
    {
        return VectorRounder.RoundVector(col.transform.position) == VectorRounder.RoundVector(transform.position);
    }
}
