using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliteCat : MonoBehaviour
{
    public GameObject beforeBody1;
    public GameObject beforeBody2;

    public GameObject afterBody1;
    public GameObject afterBody2;

    private bool isEnded = false;
    private int clickCount = 0;

    private float animationIntervalTimer = 0f;
    private int animationIndex = 0;

    private void Awake() {
        beforeBody1.SetActive(true);
        beforeBody2.SetActive(false);

        afterBody1.SetActive(false);
        afterBody2.SetActive(false);
    }

    private void Update() {
        animationIntervalTimer += Time.deltaTime;
        if(animationIntervalTimer > 0.3f) {
            animationIntervalTimer = 0f;
            animationIndex++;

            if(isEnded) {
                afterBody1.SetActive(animationIndex % 2 == 1);
                afterBody2.SetActive(animationIndex % 2 == 0);
                beforeBody1.SetActive(false);
                beforeBody2.SetActive(false);
            } else {
                beforeBody1.SetActive(animationIndex % 2 == 1);
                beforeBody2.SetActive(animationIndex % 2 == 0);
                afterBody1.SetActive(false);
                afterBody2.SetActive(false);
            }
        }
    }

    private void OnMouseDown()
    {
        if (isEnded)
        {
            return;
        }

        clickCount++;

        if(clickCount >= 5) {
            isEnded = true;
            if(MainScene.Instance == null)
            {
                return;
            }

            MainScene.Instance.Score += 100;
            MainScene.Instance.Invoke("LoadNextGame", 3f);
        }
    }
}
