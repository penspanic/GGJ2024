using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public GameObject hangingCat;

    void Start()
    {
        hangingCat.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FishingCat"))
        {
            collision.gameObject.SetActive(false);
            hangingCat.SetActive(true);

            // 몇 초 뒤에 다음씬
        }
    }
}
