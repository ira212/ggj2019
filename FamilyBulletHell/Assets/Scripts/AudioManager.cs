using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    #region Singleton
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AudioManager();
            }

            return _instance;
        }
    }
    #endregion

    private Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

    private List<AudioSource> _playingMusic = new List<AudioSource>();

    private bool _keepFadingIn;
    private bool _keepFadingOut;

    private WaitForSeconds _fadeDelay = new WaitForSeconds(0.1f);

    private bool _isSFXEnabled = true;
    public bool IsSFXEnabled
    {
        get
        {
            return _isSFXEnabled; 
        }
        private set
        {
            _isSFXEnabled = value;
        }
    }

    private bool _isMusicEnabled = true;
    public bool IsMusicEnabled
    {
        get
        {
            return _isMusicEnabled;
        }
        private set
        {
            _isMusicEnabled = value;
        }
    }

    public void Clean()
    {
        _audioSources.Clear();
    }

    public void AddAudioSources(Dictionary<string, AudioSource> audioSources)
    {
        foreach (KeyValuePair<string, AudioSource> pair in audioSources)
        {
            string name = pair.Key.ToLower();
            if (!_audioSources.ContainsKey(name))
            {
                _audioSources.Add(name, pair.Value);
            }            
        }
    }

    public void RemoveAudioSources(Dictionary<string, AudioSource> audioSources)
    {
        foreach (KeyValuePair<string, AudioSource> pair in audioSources)
        {
            string name = pair.Key.ToLower();
            if (_audioSources.ContainsKey(name))
            {
                _audioSources.Remove(name);                
            }
        }
    }

    public void PlaySFX(string sfxID, float fadeDuration = 0.25f)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(sfxID.ToLower(), out source))
        {
            source.Play();
        }
    }

    public void StopSFX(string sfxID, float fadeDuration = 0.25f)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(sfxID.ToLower(), out source))
        {
            source.Stop();
        }
    }

    public void PlayMusic(string musicID, float vol = 1.0f, float fadeDuration = 0.25f)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(musicID.ToLower(), out source))
        {
            source.Play();
            source.volume = vol;
            _playingMusic.Add(source);
        }
    }

    public void StopMusic(string musicID, float fadeDuration = 0.25f)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(musicID.ToLower(), out source))
        {
            source.Stop();
        }
    }

    public void AdjustVolume(string audioID, float newVolume)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(audioID.ToLower(), out source))
        {
            source.volume = newVolume;
        }
    }

    public bool IsPlaying(string audioID)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(audioID.ToLower(), out source))
        {
            return source.isPlaying;
        }

        return false;
    }

    public float GetDuration(string audioID)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(audioID.ToLower(), out source))
        {
            return source.clip.length;
        }

        return 0.0f;
    }

    public void Pause(string audioID)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(audioID.ToLower(), out source))
        {
            source.Pause();
        }
    }

    public void UnPause(string audioID)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(audioID.ToLower(), out source))
        {
            source.UnPause();
        }
    }

    public void ToggleMusic(bool toggle)
    {
        IsMusicEnabled = toggle;

        foreach (AudioSource source in _playingMusic)
        {
            source.volume = IsMusicEnabled ? 1.0f : 0.0f;
        }
    }

    public void ToggleSFX(bool toggle)
    {
        IsSFXEnabled = toggle;
    }

    private IEnumerator FadeIn(string audioID, float speed, float maxVolume)
    {
        _keepFadingIn = true;
        _keepFadingOut = false;

        string name = audioID.ToLower();
        _audioSources[name].volume = 0;

        while (_audioSources[name].volume < maxVolume && _keepFadingIn)
        {
            _audioSources[name].volume += speed;
            yield return _fadeDelay;
        }
    }

    private IEnumerator FadeOut(string audioID, float speed)
    {
        _keepFadingIn = false;
        _keepFadingOut = true;

        string name = audioID.ToLower();
        while (_audioSources[name].volume >= speed && _keepFadingOut)
        {
            _audioSources[name].volume -= speed;
            yield return _fadeDelay;
        }

        _audioSources[name].Stop();
    }
}