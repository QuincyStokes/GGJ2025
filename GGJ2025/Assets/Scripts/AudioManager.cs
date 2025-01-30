
using System.Collections;
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
        //jukeboxAudioSource.Stop();
    }

    public IEnumerator FadeOutMusic(AudioSource source, float time)
    {
        float elapsed = 0f;
        while(elapsed <= time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;
            source.volume = 1-t;
            yield return null;
        }
        source.volume = 0;
    }

    public IEnumerator FadeInMusic(AudioClip clip, AudioSource source, float time)
    {
        source.volume = 0f;
        source.clip = clip;
        float elapsed = 0f;
        source.Play();
        while (elapsed <= time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;

            source.volume = t;
            yield return null;
        }
        source.volume = 1;
    }  


     public IEnumerator CrossfadeInNewMusic(AudioClip clip, AudioSource jukeboxCurr, AudioSource jukeboxNew, float transitionTime )
    {
        //reduce volume of old song and raise volume of new song at the same time, this means we need two audio sources though
        float elapsed = 0f;
        jukeboxNew.clip = clip;
        jukeboxNew.volume = 0f;
        jukeboxNew.Play();
        while(elapsed <= transitionTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionTime; // current transition progress as a percentage

            //jukeboxCurr needs to go down
            //jukeboxNew needs to go up

            jukeboxCurr.volume = 1-t;
            jukeboxNew.volume = t;
            //isnt that kinda it?
            yield return null;
        }
        //jukebox can handle 
        jukeboxCurr.Stop();
    }
}
