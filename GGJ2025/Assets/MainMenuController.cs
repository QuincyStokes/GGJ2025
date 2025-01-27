using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    //whats the idea here
    
    //play the whole spaceship launch 
    //fade in the buttons and background

    [Header("UI References")]
    public VideoPlayer videoPlayer;
    public Image backgroundImage;

    void Start()
    {
        videoPlayer.enabled = true; //this will play it automatically
    }
}
