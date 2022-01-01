using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCharm : CharmOfDamageModification
{
    [SerializeField] RuntimeAnimatorController Controller; 

    public RuntimeAnimatorController GetController()
    {
        return Controller; 
    }

}
