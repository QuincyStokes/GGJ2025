using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ButtonFunctions : MonoBehaviour
{
    [Header("UI Click Sound")]
    public AudioClip UIClick;
    public AudioMixerGroup UIamg;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayUIClick()
    {
        AudioManager.Instance.PlayOneShotVariedPitch(UIClick, .5f, UIamg, .05f);
    }
}
