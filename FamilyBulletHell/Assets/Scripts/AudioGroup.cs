using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGroup : MonoBehaviour
{
    private Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

    /// <summary>
    /// Unity derived method
    /// </summary>
    private void Awake()
    {
        foreach (var audioSource in transform.GetComponentsInChildren<AudioSource>(true))
        {
            _audioSources.Add(audioSource.gameObject.name, audioSource);
        }

        AudioManager.Instance.AddAudioSources(_audioSources);
    }

    /// <summary>
    /// Unity derived method
    /// </summary>
    private void OnDestroy()
    {
        AudioManager.Instance.RemoveAudioSources(_audioSources);

        _audioSources.Clear();
    }
}
