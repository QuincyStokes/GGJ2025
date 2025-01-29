using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrbitManager : MonoBehaviour
{
    public static OrbitManager Instance;

    [Header("Orbit Settings")]
    //THIS TIME IS IN SECONDS
    public float orbitLength;

    [Header("References")]
    public GameObject clockHand;

    [Header("UI")]
    public TMP_Text dayCounter;
    public TMP_Text orbitDayText;
    public TMP_Text orbitTimeText;
    public AudioClip gameplayMusic;
    

    private bool paused;
    

    //private tings
    [HideInInspector] public int currentOrbitDay;
    private float currentOrbitTime;

    void Start()
    {
        currentOrbitDay = 1;
    }   

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Jukebox.Instance.FadeInMusic(gameplayMusic, 2f);
    }

    void Update()
    {
        if(!paused)
        {
            ProgressOrbit();
            UpdateTimeUI();
        }
        
    }

  

    private void ProgressOrbit()
    {
        if(currentOrbitTime < orbitLength)
        {
            currentOrbitTime += Time.deltaTime;
        }
        else
        {
            currentOrbitTime = 0;
            //here signals the end of the day
            //at the end of day 1, it will play cutscene[0], which is good i thnk
            print("calling EnterCutscene with index " + (currentOrbitDay - 1));
            CutsceneManager.Instance.EnterCutscene(currentOrbitDay-1);
            currentOrbitDay++;
            
        }
    }

        private void UpdateTimeUI()
        {
            
            float degreesPerSecond = 360/orbitLength; //probably wrong
            float rotationThisFrame = -(degreesPerSecond * Time.deltaTime);
            clockHand.transform.Rotate(0f, 0f, rotationThisFrame);

            dayCounter.text = "Day " + currentOrbitDay;

            //"debug" text
            orbitDayText.text = "Current Orbit Day: " + currentOrbitDay.ToString();
            orbitTimeText.text = "Current Orbit Time: " +currentOrbitTime.ToString();
        }

        public void PauseOrbit()
        {
            paused = true;
        }

        public void ResumeOrbit()
        {
            paused = false;
            // call new music here?
            //ukebox.Instance.CrossfadeIn(gameplayMusic);
        }
    

}
