using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro; 

public class LastLetterPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Letter;

    private string path = "king";

    public void EndGame()
    {
        LoadScript.LoadTo(Scenes.CreditsScene, "");
    }

    public void Show()
    {
        string letterText = File.ReadAllText(MainMenuScript.appendPath + path);
        Letter.text = letterText;
        
        gameObject.SetActive(true);
    }
}
