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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check 5sec
        if (Time.time - startTime < 5f)
        {
            return;
        }

        if (collision.gameObject.CompareTag("FishingCat"))
        {
            collision.gameObject.SetActive(false);
            hangingCat.SetActive(true);

            // 몇 초 뒤에 다음씬
        }
    }
}
