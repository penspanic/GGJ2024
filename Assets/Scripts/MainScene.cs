using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UI;

public class MainScene : MonoBehaviour 
{
    public Text scoreText;
    public Curtain curtain;

    private static MainScene instance;

    public static MainScene Instance 
    {
        get {
            // if (instance == null)
            // {
            //     instance = FindObjectOfType<MainScene>();
            //
            //     if (instance == null)
            //     {
            //         GameObject obj = new GameObject();
            //         obj.name = typeof(MainScene).Name;
            //         instance = obj.AddComponent<MainScene>();
            //     }
            // }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            curtain.gameObject.SetActive(true);
            LoadNextGame();
            ScreenFade.Instance.FadeIn();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadNextGame();
        }
    }

    private readonly string GAME_SCENE_PREFIX = "GameScene";
    [SerializeField] private GameObject sceneButtonsRoot;

    private string lastLoadedSceneName = string.Empty;

    private int score;
    public int Score 
    {
        get => score;
        set
        {
            int previousValue = score;
            score = value;
            OnScoreChanged?.Invoke(previousValue, score);
            if(scoreText) 
            {
                scoreText.text = score.ToString();
            }
        }
    }

    public delegate void OnScoreChangedDelegate(int previousScore, int newScore);

    public event Action<int, int> OnScoreChanged;

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

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        this.lastLoadedSceneName = sceneName;
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

    private int currentGameIndex = -1;
    private IEnumerator LoadNextGameRoutine() {
        if (currentGameIndex == 3) // 임시 테스트
        {
            curtain.AllEnd();
            OnGameClear();
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(curtain.ShowCredit());
            yield return new WaitForSeconds(2f);
            ScreenFade.Instance.FadeIn();
            SceneManager.LoadScene("StartScene");
            yield break;
        }
        // curtain animation
        if (currentGameIndex != -1)
        {
            curtain.Close();
            yield return new WaitForSeconds(1.5f);
        }
        string nextSceneName = null;
        if (currentGameIndex == -1)
            nextSceneName = "CatBelly";
        if (currentGameIndex == 0)
            nextSceneName = "CatFishing";
        if (currentGameIndex == 1)
            nextSceneName = "CatHuh";
        if (currentGameIndex == 2)
            nextSceneName = "CatPolite";

        ++currentGameIndex;

        LoadAdditiveScene(nextSceneName);
        yield return new WaitForSeconds(1f);
        curtain.Down();
    }

    public void OnGameClear()
    {
        CreditController.SetCreditMenuVisible();
    }
}