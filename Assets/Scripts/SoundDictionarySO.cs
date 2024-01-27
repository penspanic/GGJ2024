using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDictionary", menuName = "ScriptableObjects/SoundDictionary", order = 1)]
public class SoundDictionarySO : ScriptableObject
{
    [System.Serializable]
    public class SoundItem
    {
        public string key;
        public AudioClip audioClip;
    }

    public List<SoundItem> soundList = new List<SoundItem>();

    private Dictionary<string, AudioClip> soundDictionary;

    public Dictionary<string, AudioClip> GetDictionary()
    {
        if (soundDictionary == null)
            BuildDictionary();

        return soundDictionary;
    }

    private void BuildDictionary()
    {
        soundDictionary = new Dictionary<string, AudioClip>();

        foreach (SoundItem soundItem in soundList)
        {
            if (!string.IsNullOrEmpty(soundItem.key) && soundItem.audioClip != null)
            {
                soundDictionary[soundItem.key] = soundItem.audioClip;
            }
        }
    }
}
