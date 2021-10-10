using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Transform Player; 

    public void FixedUpdate()
    {
        transform.position = ((Vector2)Player.position);
    }
}
