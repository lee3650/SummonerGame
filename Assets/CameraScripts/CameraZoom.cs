using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] PixelPerfectCamera Camera;
    [SerializeField] float Sensitivity;

    float refMultipler = 1f;
    int referenceX;
    int referenceY; 

    private void Awake()
    {
        referenceX = Camera.refResolutionX;
        referenceY = Camera.refResolutionY;
    }

    void Update()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            refMultipler += Sensitivity * Input.mouseScrollDelta.y;

            if (refMultipler < 0.25f)
            {
                refMultipler = 0.25f;
            }
            if (refMultipler > 1f) 
            {
                refMultipler = 1f;
            }

            print("ref multiplier: " + refMultipler);

            Camera.refResolutionX = nearestEven(refMultipler * referenceX);
            Camera.refResolutionY = nearestEven(refMultipler * referenceY);

            print("x: " + Camera.refResolutionX + ", y: " + Camera.refResolutionY);
        }
    }

    int nearestEven(float input)
    {
        int result = Mathf.RoundToInt(input);

        if (result % 2 == 0)
        {
            return result; 
        }
        return result + 1;
    }
}
