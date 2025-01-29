using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Jukebox : MonoBehaviour
{
    //instance to play from elsewhere (and eventually probably pause/interrupt)
    public static Jukebox Instance;
    [Header("Background Music")]
    public List<AudioClip> backgroundMusic;
    public AudioSource jukeboxOne;
    public AudioSource jukeboxTwo;
    public AudioSource currentJukebox;
    public AudioMixerGroup amg;
    public AudioClip day4CutsceneSong;
    private bool musicPlaying;
    private bool manualPause;
    private AudioClip currentSong;

    void Awake()
    {
        if(Instance == null)
        {
            Instance =  this;
            currentJukebox = jukeboxOne;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
  
    void PlayMusic()
    {
        if(backgroundMusic.Count == 0)
        {
            print("No music to play.");
            return;
        }
        else if (backgroundMusic.Count == 1) {
            AudioManager.Instance.PlayOneShot(backgroundMusic[0], 1f, amg, currentJukebox);
            currentSong = backgroundMusic[0];
            StartCoroutine(SongCooldown());
        }
        else
        {
            //pick a random song
            AudioClip newSong = backgroundMusic[Random.Range(0, backgroundMusic.Count)];

            //if the song we chose is the one we just played, pick a different one
            if(newSong == currentSong)
            {
                PlayMusic();
            }
            else //we got a new song! play it.
            {
                AudioManager.Instance.PlayOneShot(newSong, 1f, amg, currentJukebox);
                currentSong = newSong;
                StartCoroutine(SongCooldown());
            }
        }
       
    }

    public void PlaySong(AudioClip clip)
    {
        //fade out current song and play new one
        AudioManager.Instance.StopAllSounds();
        musicPlaying = true;
        AudioManager.Instance.PlayOneShot(clip, 1f, amg, currentJukebox);
        
    }

    private IEnumerator SongCooldown()
    {
        musicPlaying = true;
        yield return new WaitForSeconds(currentSong.length);
        musicPlaying = false;

    }

    public void FadeOutMusic(float time)
    {
        StartCoroutine(AudioManager.Instance.FadeOutMusic(currentJukebox, time));

    }

    public void FadeInMusic(AudioClip clip, float time)
    {
        StartCoroutine(AudioManager.Instance.FadeInMusic(clip, currentJukebox, time ));
        currentSong = clip;
        StartCoroutine(SongCooldown());
    }


    public void ResumeMusic()
    {
        manualPause = false;
        musicPlaying = false;
    }

    public void CrossfadeIn(AudioClip clip)
    {
        if(currentJukebox = jukeboxOne)
        {
            StartCoroutine(AudioManager.Instance.CrossfadeInNewMusic(clip, jukeboxOne, jukeboxTwo, 3f));
            currentJukebox = jukeboxTwo;
        }
        else
        {
            StartCoroutine(AudioManager.Instance.CrossfadeInNewMusic(clip, jukeboxTwo, jukeboxOne, 3f));
            currentJukebox = jukeboxOne;
        }
    }


}
