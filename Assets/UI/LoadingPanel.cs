using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    float hideTime = 1f;

    public void ShowLoadingPanel(string message)
    {
        loadingText.text = message;
        gameObject.SetActive(true);
    }

    public void HideLoadingPanel()
    {
        StopAllCoroutines();
        StartCoroutine(Hide(hideTime));
    }

    private IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
