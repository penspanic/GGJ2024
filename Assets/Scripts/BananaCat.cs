using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCat : MonoBehaviour
{
    public GameObject normal1;
    public GameObject normal2;

    public GameObject after1;
    public GameObject after2;

    private float animationIntervalTimer = 0f;
    private int animationIndex = 0;

    private bool isHit = false;

    void Awake()
    {
        normal1.SetActive(true);
        normal2.SetActive(false);

        after1.SetActive(false);
        after2.SetActive(false);
    }

    public void Hit() {
        isHit = true;
    }

    void Update() {
        animationIntervalTimer += Time.deltaTime;
        if(animationIntervalTimer > 0.3f) {
            animationIntervalTimer = 0f;
            animationIndex++;

            if(isHit) {
                after1.SetActive(animationIndex % 2 == 1);
                after2.SetActive(animationIndex % 2 == 0);
                normal1.SetActive(false);
                normal2.SetActive(false);
            } else {
                normal1.SetActive(animationIndex % 2 == 1);
                normal2.SetActive(animationIndex % 2 == 0);
                after1.SetActive(false);
                after2.SetActive(false);
            }
        }
    }
}
