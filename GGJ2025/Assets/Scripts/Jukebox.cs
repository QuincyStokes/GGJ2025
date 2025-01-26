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
    public AudioMixerGroup amg;

    private bool musicPlaying;
    private bool manualPause;
    private AudioClip currentSong;

    void Update()
    {
        if(musicPlaying == false && manualPause == false)
        {
            PlayMusic();
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
            AudioManager.Instance.PlayOneShot(backgroundMusic[0], 1f, amg, AudioManager.Instance.jukeboxAudioSource);
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
                AudioManager.Instance.PlayOneShot(newSong, 1f, amg, AudioManager.Instance.jukeboxAudioSource);
                currentSong = newSong;
                StartCoroutine(SongCooldown());
            }
        }
       
    }

    private IEnumerator SongCooldown()
    {
        musicPlaying = true;
        yield return new WaitForSeconds(currentSong.length);
        musicPlaying = false;

    }

    public void StopMusic()
    {
        AudioManager.Instance.jukeboxAudioSource.Stop();
        manualPause = true;
    }


    public void ResumeMusic()
    {
        manualPause = false;
    }

}
