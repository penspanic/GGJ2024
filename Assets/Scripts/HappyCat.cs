using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyCat : MonoBehaviour
{

    public GameObject body1;
    public GameObject body2;


    private float animationIntervalTimer = 0f;
    private int animationIndex = 0;

    private bool startAnimation = false;

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
        if (animationIntervalTimer > 0.3f)
        {
            animationIntervalTimer = 0f;
            animationIndex++;

            body1.SetActive(animationIndex % 2 == 1);
            body2.SetActive(animationIndex % 2 == 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D : " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Cucumber"))
        {
            collision.gameObject.SetActive(false);
            startAnimation = true;
        }
    }

}
