
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Master Mixer")]
    public AudioMixer masterMixer;
    
    private List<AudioSource> sourcePool;
    //making a specific jukebox audio source so it can be paused/stopped
    public AudioSource jukeboxAudioSource;
    public int poolSize;



    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSourcePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //keep a pool of audio sources, reduces need to create more as many sounds play
    private void InitializeAudioSourcePool()
    {
        sourcePool = new List<AudioSource>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject("AudioSource_" + i);
            obj.transform.SetParent(transform);
            AudioSource source = obj.AddComponent<AudioSource>();
            sourcePool.Add(source);
        }
    }

    public void PlayOneShot(AudioClip clip, float volume, AudioMixerGroup mixerGroup, AudioSource audioSource = null)
    {
        AudioSource source;
        if(audioSource == null)
        {
            source = GetAvailableSource();
        }
        else{
            source = audioSource;
        }
        
        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.volume = volume;
        source.spatialBlend = 0; // 2D by default; adjust as needed
        source.PlayOneShot(clip);
    }

    private AudioSource GetAvailableSource()
    {
        // Return first available or oldest if none free
        foreach (AudioSource source in sourcePool)
        {
            if (!source.isPlaying)
                return source;
        }
        // If all are playing, return the first one
        return sourcePool[0];
    }


    public void PlayOneShotVariedPitch(AudioClip clip, float volume, AudioMixerGroup mixerGroup, float pitchOffset)
    {
        AudioSource source = GetAvailableSource();
        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.volume = volume;
        source.spatialBlend = 0f; // 2D by default; adjust as needed
        source.pitch = Random.Range(1-pitchOffset, 1+pitchOffset);
        source.PlayOneShot(clip);
    }

    public void StopAllSounds()
    {
        foreach(AudioSource source in sourcePool)
        {
            source.Stop();
        }
        jukeboxAudioSource.Stop();
    }
}
