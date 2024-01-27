using System;
using System.Collections;
using UnityEngine;

public class Curtain : MonoBehaviour
{
    public Animator animator;
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
}