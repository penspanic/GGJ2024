using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour 
{
    private readonly string MAIN_SCENE = "MainScene";
    public GameObject creditButton;
    public CanvasGroup creditPanelGroup;
    private bool isCreditAnimation = false;

    private void Awake()
    {
        creditButton.gameObject.SetActive(CreditController.IsCreditMenuVisible());
    }

    public void OnStartGame() 
    {
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public void OnCredit()
    {
        creditPanelGroup.gameObject.SetActive(true);
        isCreditAnimation = true;
        creditPanelGroup.alpha = 0f;
        var tween = creditPanelGroup.DOFade(1f, 0.5f);
        tween.onComplete += () => isCreditAnimation = false;
        tween.Play();
    }

    private void Update()
    {
        if (isCreditAnimation == false && Input.GetMouseButtonDown(0))
        {
            creditPanelGroup.gameObject.SetActive(false);
        }
    }

    public void OnQuitGame() 
    {
        Application.Quit();
    }
}
