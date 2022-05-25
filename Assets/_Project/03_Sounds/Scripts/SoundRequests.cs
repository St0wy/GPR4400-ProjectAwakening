using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SoundRequest", menuName = "ScriptableObjects/SoundRequests", order = 4)]
public class SoundRequests : ScriptableObject
{
    public UnityAction<AudioClip> OnRequest = null;

    public void Request(AudioClip clip)
    {
        if (clip != null)
            OnRequest?.Invoke(clip);
    }
}