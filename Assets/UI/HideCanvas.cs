using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCanvas : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Canvas.SetActive(!Canvas.activeSelf);
        }
    }
}
