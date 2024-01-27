using System;
using System.Collections;
using UnityEngine;

public class Curtain : MonoBehaviour
{
    public Animator animator;
    public GameObject credit;
    public void Awake()
    {
    }

    public void Close()
    {
        animator.Play("Curtain_Close");
    }

    public void Open()
    {
        animator.Play("Curtain_Open");
    }

    public void Down()
    {
        animator.Play("Curtain_Down");
    }

    public void AllEnd()
    {
        animator.Play("Curtain_AllEnd");
    }

    public IEnumerator ShowCredit()
    {
        credit.SetActive(true);
        float endY = 6.31f;
        float startY = -2.6f;
        float totalTime = 3f;
        float time = 0f;
        while (time < totalTime)
        {
            time += Time.deltaTime;
            float y = Mathf.Lerp(startY, endY, time / totalTime);
            credit.transform.localPosition = new Vector3(0, y, 0);
            yield return null;
        }
    }
}