using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public GameObject hangingCat;
    private float startTime;

    void Start()
    {
        hangingCat.SetActive(false);
        startTime = Time.time;
    }

    private bool nextSceneRequested = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check 5sec
        if (Time.time - startTime < 5f)
        {
            return;
        }

        if (collision.gameObject.CompareTag("FishingCat"))
        {
            if(collision.gameObject.GetComponent<FishingCat>().isJumping == false)
            {
                return;
            }
            collision.gameObject.SetActive(false);
            hangingCat.SetActive(true);

            if (nextSceneRequested)
                return;
            nextSceneRequested = true;

            MainScene.Instance.AddScoreMulti(5);
            MainScene.Instance.Invoke("LoadNextGame", 2f);
        }
    }
}
