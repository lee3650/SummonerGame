using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LoadingText;
    [SerializeField] float minWaitTime = 1.5f;
    private static Scenes NextScene;
    private static string loadMessage;

    public const string LoadSceneName = "LoadingScene";

    private void Awake()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        LoadingText.text = loadMessage;
        yield return new WaitForSeconds(minWaitTime);
        SceneManager.LoadScene(GetNextSceneName(NextScene));
    }

    private string GetNextSceneName(Scenes nextScene)
    {
        switch (nextScene)
        {
            case Scenes.GameplayScene:
                return "SetupScene";
            case Scenes.MainMenu:
                return "MainMenu";
            case Scenes.ProgressionMenu:
                return "ProgressionMenu";
        }
        throw new System.Exception("Could not get next scene name! nextScene = " + nextScene.ToString());
    }

    public static void LoadTo(Scenes scene, string message)
    {
        NextScene = scene;
        loadMessage = message;
        SceneManager.LoadScene(LoadSceneName);
    }
}
