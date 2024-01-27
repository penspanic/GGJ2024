using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public SoundDictionarySO soundDictionary;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSource();
        }
        else
        {
            Destroy(gameObject);
        }
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
