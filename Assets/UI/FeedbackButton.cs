using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackButton : MonoBehaviour
{
    public void OpenSurvey()
    {
        System.Diagnostics.Process.Start("https://forms.gle/WrTReYJiEQyZhch17");
    }
}
