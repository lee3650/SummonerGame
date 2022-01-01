using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class CreditsScene : MonoBehaviour
{
    [SerializeField] float delayTime;
    [SerializeField] float firstDelay;
    [SerializeField] string FileName;
    [SerializeField] TextMeshProUGUI Credits;
    [SerializeField] GameObject Logo;
    string[] creditsText;

    int cur = 0;
    float timer;

    private void Awake()
    {
        creditsText = File.ReadAllText(MainMenuScript.appendPath + FileName).Split('\\');
        timer = firstDelay;
        Credits.text = creditsText[cur];
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Logo.SetActive(false);
            timer = delayTime;
            if (cur < creditsText.Length - 1)
            {
                cur++;
                Credits.text = creditsText[cur].Trim();
            }
        }

        if (cur == creditsText.Length - 1)
        {
            if (Input.anyKeyDown)
            {
                LoadScript.LoadTo(Scenes.MainMenu, "Loading...");
            }
        }
    }
}
