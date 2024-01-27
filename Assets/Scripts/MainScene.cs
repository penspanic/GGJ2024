using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainScene : MonoBehaviour 
{
    private readonly string GAME_SCENE_PREFIX = "GameScene";
    [SerializeField] private GameObject sceneButtonsRoot;

    private string lastLoadedSceneName = string.Empty;

    private void Start() 
    {
        var buttons = sceneButtonsRoot.GetComponentsInChildren<Button>();
        foreach (var button in buttons) 
        {
            button.onClick.AddListener(() => OnSceneButtonClicked(button.name));
        }
    }

    private void OnSceneButtonClicked(string sceneName) 
    {
        LoadAdditiveScene(sceneName);
    }

    public void LoadAdditiveScene(string sceneName)
    {
        StartCoroutine(LoadAdditiveSceneRoutine(sceneName));
    }

    private IEnumerator LoadAdditiveSceneRoutine(string sceneName)
    {
        if (IsSceneLoaded(lastLoadedSceneName))
        {
            yield return StartCoroutine(UnloadScene(lastLoadedSceneName));
        }

        var sceneFullName = GAME_SCENE_PREFIX + sceneName;
        SceneManager.LoadScene(sceneFullName, LoadSceneMode.Additive);
        this.lastLoadedSceneName = sceneFullName;
    }

    private IEnumerator UnloadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    private bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }
}
