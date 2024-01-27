using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainScene : MonoBehaviour 
{
    public Text scoreText;

    private static MainScene instance;

    public static MainScene Instance 
    {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<MainScene>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(MainScene).Name;
                    instance = obj.AddComponent<MainScene>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private readonly string GAME_SCENE_PREFIX = "GameScene";
    [SerializeField] private GameObject sceneButtonsRoot;

    private string lastLoadedSceneName = string.Empty;

    private int score;
    public int Score 
    {
        get => score;
        set {
            score = value;
            if(scoreText) 
            {
                scoreText.text = score.ToString();
            }
        }
    }

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

    public void LoadNextGame() {
        StartCoroutine(LoadNextGameRoutine());
    }

    private IEnumerator LoadNextGameRoutine() {
        // curtain animation

        yield return new WaitForSeconds(3f); 

        var sceneName = SceneManager.GetActiveScene().name;
        var sceneNumber = int.Parse(sceneName.Substring(GAME_SCENE_PREFIX.Length));
        sceneNumber++;
        if (sceneNumber > 3) {
            sceneNumber = 1;
        }
        LoadAdditiveScene(sceneNumber.ToString());
    }
}
