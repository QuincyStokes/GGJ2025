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
    }

    void Update()
    {
        ProgressOrbit();
        UpdateTimeUI();
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
    

}
