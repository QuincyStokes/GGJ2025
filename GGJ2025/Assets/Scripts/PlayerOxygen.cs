using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class PlayerOxygen : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygen;
    private float currentOxygen;
    public float oxygenLossRate;
    public bool paused;


    [Header("UI Elements")]
    public Image oxygenBar;

    void Update()
    {
        if(!paused)
        {
            ReduceOxygen(oxygenLossRate);
        }
        else
        {

        }
    }

    void Start()
    {
        currentOxygen = maxOxygen;
        paused = false;
    }


    public void ReduceOxygen(float amount)
    {
        currentOxygen -= amount;
        UpdateOxygenBar();
        if(currentOxygen <= 0)
        {
            //player is dead
            Die();
        }
    }

    public void IncreaseOxygen(float amount)
    {
        currentOxygen += amount;
        if(currentOxygen > maxOxygen)
        {
            currentOxygen = maxOxygen;
        }
        UpdateOxygenBar();
    }

    void UpdateOxygenBar()
    {
        oxygenBar.transform.localScale = new Vector3(1, currentOxygen/maxOxygen, 1);
    }
    
    private void Die()
    {
        //we need to return to main menu
        AudioManager.Instance.StopAllSounds();
        SceneManager.LoadScene("DeathScene");
        //SceneManager.LoadScene("MenuScreen");
    }

    public void PauseOxygen()
    {
        paused = true;
        oxygenBar.enabled = false;
    }

    public void ResumeOxygen()
    {
        paused= false;
        oxygenBar.enabled = true;
    }



}
