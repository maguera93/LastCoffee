using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private AudioClip[] m_clips;

    public void PlayClip(int index)
    {
        PlayClip(index, 1, 1);
    }

    public void PlayClip(int index, float volume)
    {
        PlayClip(index, volume, 1);
    }

    public void PlayClipRandomPitch(int index)
    {
        PlayClipRandomPitch(index, 1);
    }

    public void PlayClipRandomPitch(int index, float volume)
    {
        float pitch = Random.Range(0.8f, 1.2f);
        PlayClip(index, volume, pitch);
    }

    public void PlayClip(int index, float volume, float pitch)
    {
        GameObject obj = new GameObject();
        AudioSource s = obj.AddComponent<AudioSource>();
        s.clip = m_clips[index];
        s.volume = volume;
        s.pitch = pitch;
        s.Play();
        Destroy(obj, m_clips[index].length);
    }
}
