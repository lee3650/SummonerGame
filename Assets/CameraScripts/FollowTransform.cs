using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] Transform Transform;

    private void FixedUpdate()
    {
        transform.position = new Vector3(Transform.position.x, Transform.position.y, -10f);    
    }
}
