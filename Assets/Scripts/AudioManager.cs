using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>("AudioManager")).GetComponent<AudioManager>();
            }
            return instance;
        }
    }

    private static AudioManager instance;

    public SoundDictionarySO soundDictionary;

    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeAudioSource();
    }

    private void InitializeAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(string soundKey)
    {
        Dictionary<string, AudioClip> dictionary = soundDictionary.GetDictionary();

        if (dictionary.ContainsKey(soundKey))
        {
            AudioClip audioClip = dictionary[soundKey];
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogError("Sound key not found in the dictionary.");
        }
    }
}
