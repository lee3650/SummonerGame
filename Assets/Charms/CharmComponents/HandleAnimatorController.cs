using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimatorController : MonoBehaviour, ISubCharmHandler
{
    [SerializeField] Animator Animator;

    public void ApplyCharm(Charm c)
    {
        switch (c)
        {
            case AnimatorCharm ac:
                Animator.runtimeAnimatorController = ac.GetController();
                break;
        }
    }
}
