using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour {
    private readonly string MAIN_SCENE = "MainScene";

    public void OnStartGame() {
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public void OnQuitGame() {
        Application.Quit();
    }
}
