
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    //whats the idea here
    
    //play the whole spaceship launch 
    //fade in the buttons and background
    [Header("Settings")]
    public float fadeTime;
    public AudioClip rocketlaunchAudio;
    public AudioClip menuMusic;

    [Header("UI References")]
    public VideoPlayer videoPlayer;

    //all of these things below need to fade in
    public Image backgroundImage;
    public Button playButton;
    public Button quitButton;
    public TMP_Text playButtonText;
    public TMP_Text quitButtonText; 

    void Start()
    {
        backgroundImage.enabled = false; //turn off background image
        playButton.interactable = false; //make button non-interactable
        playButton.gameObject.GetComponent<Image>().enabled = false; //turn off image for the button

        quitButton.interactable = false;
        quitButton.gameObject.GetComponent<Image>().enabled = false;

        playButtonText.enabled = false; //turn off text of button
        quitButtonText.enabled = false;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Spaceshiplaunch.mp4");
        videoPlayer.enabled = true; //this will play it automatically
        Jukebox.Instance.PlaySong(rocketlaunchAudio);
        //videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if(videoPlayer.frame >= (long)(videoPlayer.frameCount/2))
        {
            StartCoroutine(FadeInMainMenu());   
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        //this is when video finishes playing, should fade in title screen + buttons now
        
    }

    private IEnumerator FadeInMainMenu()
    {
        //perhaps we first fade out the videoplayer object?
        //play main menu music

        Jukebox.Instance.FadeOutMusic(1f);
        float elapsed = 0f;
        while(elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;
            videoPlayer.targetCameraAlpha = 1-t;
            yield return null;
        }
        //should just disable video player here
        videoPlayer.enabled = false;

        //first, set all their alpha values to 0
        Color noAlpha = new Color(1f, 1f, 1f, 0);
        backgroundImage.color = noAlpha;
        playButton.gameObject.GetComponent<Image>().color = noAlpha;
        quitButton.gameObject.GetComponent<Image>().color = noAlpha;
        playButtonText.color = noAlpha;
        quitButtonText.color = noAlpha;

        //enable them all

        backgroundImage.enabled = true; 
        
        playButton.gameObject.GetComponent<Image>().enabled = true; 
        quitButton.gameObject.GetComponent<Image>().enabled = true;

        playButtonText.enabled = true; 
        quitButtonText.enabled = true;

        //now everything should be set to fully opaque and enabled, can start fading in
        elapsed = 0f;
        Jukebox.Instance.FadeInMusic(menuMusic, 1f);
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;
            Color tColor = new Color(1f, 1f, 1f, t);
            backgroundImage.color = tColor;
            playButton.gameObject.GetComponent<Image>().color = tColor;
            quitButton.gameObject.GetComponent<Image>().color = tColor;
            playButtonText.color = tColor;
            quitButtonText.color = tColor;
            yield return null;
        }

        //at the end, set all the opacities just to be sure
        Color fullAlpha = new Color(1f, 1f, 1f, 1f);
        backgroundImage.color = fullAlpha;
        playButton.gameObject.GetComponent<Image>().color = fullAlpha;
        quitButton.gameObject.GetComponent<Image>().color = fullAlpha;
        playButtonText.color = fullAlpha;
        quitButtonText.color = fullAlpha;
        //should be fully transparent again
        //set the buttons to interactable;
        quitButton.interactable = true;
        playButton.interactable = true;

    }
}
