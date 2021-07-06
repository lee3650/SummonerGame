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
        if (collision.TryGetComponent<CoatingManager>(out coatingManager))
        {
            coatingManager.SetCoating(Coating.GetCoating(CoatingType, CoatingDuration));
        }
    }
}
