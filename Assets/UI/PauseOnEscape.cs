using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnEscape : MonoBehaviour
{
    [SerializeField] Pause PauseMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.TogglePauseGame();
        }
    }
}
