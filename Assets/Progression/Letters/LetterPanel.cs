using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class LetterPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Letter;
    [SerializeField] TextMeshProUGUI Date; 

    public void ShowLetter(LetterUnlock l)
    {
        Date.text = l.Date; 
        string letterText = File.ReadAllText(MainMenuScript.appendPath + l.path);
        Letter.text = letterText;
        gameObject.SetActive(true);
    }
}
