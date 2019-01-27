using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject); //makes instance persist across scenes
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); //deletes copies of global which do not need to exist, so right version is used to get info from
        }
    }
    #endregion

    private Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

    private List<AudioSource> _playingMusic = new List<AudioSource>();

    private bool _keepFadingIn;
    private bool _keepFadingOut;

    private WaitForSeconds _fadeDelay = new WaitForSeconds(0.1f);

    private MonoBehaviour _coroutineStarter = new MonoBehaviour();

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

    public void PlayMusic(string musicID, float fadeDuration = 0.25f)
    {
        AudioSource source;
        if (_audioSources.TryGetValue(musicID.ToLower(), out source))
        {
            source.Play();
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

    public void TransitionTracks(string track1, string track2, float fadeDuration = 1.5f)
    {
        int iterations = (int)(fadeDuration / 0.1);
        float speed = (float)1 / iterations;
        StartCoroutine(TransitionTrackCoroutine(_audioSources[track1.ToLower()], _audioSources[track2.ToLower()], iterations, speed, 1));
    }

    private IEnumerator TransitionTrackCoroutine(AudioSource source1, AudioSource source2, int iterations, float speed, float maxVolume)
    {
        source2.volume = 0;

        while (source1.volume >= speed && source2.volume < maxVolume)
        {
            source2.volume += speed;
            source1.volume -= speed;
            yield return _fadeDelay;
        }
        
        if (source2.volume < maxVolume)
        {
            source2.volume = maxVolume;
        }
        if (source1.volume > 0)
        {
            source1.volume = 0;
        }
    }

    public void FadeIn(string audioID, float fadeDuration = 0.5f)
    {
        int iterations = (int)(fadeDuration / 0.1);
        float speed = (float)1 / iterations;
        StartCoroutine(FadeInCoroutine(_audioSources[audioID.ToLower()], iterations, speed, 1));
    }

    private IEnumerator FadeInCoroutine(AudioSource source, int iterations, float speed, float maxVolume)
    {
        _keepFadingIn = true;
        source.volume = 0;

        while (source.volume < maxVolume && _keepFadingIn)
        {
            source.volume += speed;
            yield return _fadeDelay;
        }

        if (source.volume < maxVolume)
        {
            source.volume = maxVolume;
        }
    }

    public void FadeOut(string audioID, float fadeDuration = 0.5f)
    {
        int iterations = (int)(fadeDuration / 0.1);
        float speed = (float)1 / iterations;
        StartCoroutine(FadeOutCoroutine(_audioSources[audioID.ToLower()], iterations, speed));
    }

    private IEnumerator FadeOutCoroutine(AudioSource source, int iterations, float speed)
    {
        _keepFadingOut = true;

        while (source.volume >= speed && _keepFadingOut)
        {
            source.volume -= speed;
            yield return _fadeDelay;
        }
        
        source.Stop();
    }
}