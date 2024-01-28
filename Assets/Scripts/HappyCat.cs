using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyCat : MonoBehaviour
{

    public GameObject body1;
    public GameObject body2;

    public BananaCat bananaCat;

    private float animationIntervalTimer = 0f;
    private int animationIndex = 0;

    private bool startAnimation = false;

    private bool isEnd = false;
    private void Awake()
    {
        body1.SetActive(true);
        body2.SetActive(false);
    }

    private void Update()
    {
        if(!startAnimation)
        {
            return;
        }
        animationIntervalTimer += Time.deltaTime;
        if (animationIntervalTimer > 0.2f)
        {
            animationIntervalTimer = 0f;
            animationIndex++;

            body1.SetActive(animationIndex % 2 == 1);
            body2.SetActive(animationIndex % 2 == 0);
        }

        // move to right
        if(transform.position.x < 2.2f)
        {
            transform.position += Vector3.right * Time.deltaTime * 1.5f;
        } else {
            if(isEnd) {
                return;
            }
            isEnd = true;
            bananaCat.Invoke("Hit", 1f);

            if(MainScene.Instance == null)
            {
                return;
            }

            MainScene.Instance.AddScoreMulti(3);
            MainScene.Instance.Invoke("LoadNextGame", 4f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cucumber"))
        {
            collision.collider.enabled = false;
            startAnimation = true;
        }
    }

}
