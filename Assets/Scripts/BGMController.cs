using System;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>("BGMController")).GetComponent<BGMController>();
            }
            return instance;
        }
    }

    private static BGMController instance;
    public AudioSource audioSource;
    public AudioClip bgmClip;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private bool isPlaying = false;
    public void PlayBGM()
    {
        if (isPlaying)
            return;
        isPlaying = true;
        audioSource.clip = bgmClip;
        audioSource.Play();
    }
}