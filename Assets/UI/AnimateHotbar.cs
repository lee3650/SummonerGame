using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateHotbar : MonoBehaviour
{
    [SerializeField] AnimateInAndOut Animator;

    private const int ZeroIndex = (int)KeyCode.Alpha1;

    private void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)(ZeroIndex + i)))
            {
                if (!Animator.IsShown)
                {
                    Animator.ToggleVisibility();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Animator.IsShown)
            {
                Animator.ToggleVisibility();
            }
        }

        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            if (!Animator.IsShown)
            {
                Animator.ToggleVisibility();
            }
        }
    }
}
