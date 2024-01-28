using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuhCat : MonoBehaviour
{
    public GameObject head1;
    public GameObject head2;

    private bool isClicked = false;

    private void Awake() {
        head1.SetActive(true);
        head2.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (isClicked)
        {
            return;
        }

        isClicked = true;
        head1.SetActive(false);
        head2.SetActive(true);

        if(AudioManager.Instance == null || MainScene.Instance == null)
        {
            return;
        }

        MainScene.Instance.AddScoreMulti(3);
        AudioManager.Instance.PlaySound("huh");
        MainScene.Instance.Invoke("LoadNextGame", 2f);
    }
}
