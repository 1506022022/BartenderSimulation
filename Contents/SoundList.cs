using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "Scriptable Object/SoundList")]
public class SoundList : ScriptableObject
{
    public List<AudioClip> m_AudioClipList;
    public List<KeySourcePair> SoundEventList;

    private Dictionary<string, AudioClip> m_AudioClipDictionary;
    private Dictionary<string, AudioClip> m_Eventm_AudioClipDictionary;

    public Dictionary<string, AudioClip> AudioClips
    {
        get
        {
            if (m_AudioClipDictionary == null || m_AudioClipList.Count != m_AudioClipDictionary.Count)
                m_AudioClipDictionary = GetSoundList();

            return m_AudioClipDictionary;
        }
    }
    public Dictionary<string, AudioClip> EventAudioClips
    {
        get
        {
            if (m_Eventm_AudioClipDictionary == null || SoundEventList.Count != m_Eventm_AudioClipDictionary.Count)
                m_Eventm_AudioClipDictionary = GetEventSoundList();

            return m_Eventm_AudioClipDictionary;
        }
    }


    private Dictionary<string, AudioClip> GetSoundList()
    {
        Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip item in m_AudioClipList)
        {
            if (audioDictionary.ContainsKey(item.name))
                continue;
            audioDictionary.Add(item.name, item);
        }

        return audioDictionary;
    }
    private Dictionary<string, AudioClip> GetEventSoundList()
    {
        Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

        foreach (var item in SoundEventList)
        {
            if (audioDictionary.ContainsKey(item.AudioSource.name))
                continue;
            audioDictionary.Add(item.Key, item.AudioSource);
        }

        return audioDictionary;
    }

    public void PlaySound(AudioSource source, string key)
    {
        var clip = EventAudioClips[key];
        if (source.clip != clip)
        {
            source.clip = clip;
        }
        if (!source.isPlaying) source.Play();
    }
}

[Serializable]
public struct KeySourcePair
{
    public string Key;
    public AudioClip AudioSource;
}