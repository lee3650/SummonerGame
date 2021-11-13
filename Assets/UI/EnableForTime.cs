using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnableForTime : MonoBehaviour
{
    [SerializeField] float length;
    [SerializeField] TextMeshProUGUI text;

    public void Enable(string message)
    {
        text.text = message;
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(StayEnabled());
    }

    IEnumerator StayEnabled()
    {
        yield return new WaitForSeconds(length);
        gameObject.SetActive(false);
    }
}
