using System;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgmClip;

    private void Awake()
    {
        audioSource.clip = bgmClip;
        audioSource.Play();
        DontDestroyOnLoad(gameObject);
    }
}