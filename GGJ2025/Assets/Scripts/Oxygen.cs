using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{

    [SerializeField] private float currentOxygen;                   // Player's current oxygen level
    [SerializeField] private float startingOxygen = 100;            // Player's default starting oxygen level
    [SerializeField] private float maxCapacity = 1000;              // Player's max oxygen capacity
    [SerializeField] private float attackRate = 1;                  // How often the player can attack in seconds
    [SerializeField] private float oxygenGunShotCost = 1;           // How much oxygen each shot takes

    private float lastAttackedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentOxygen = startingOxygen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddOxygen(float amount)    // I like having helper functions being the ONLY way to add / remove to variables that change to keep them from being accessed by things they shouldn't. Allows us to keep them private as well.
    {
        currentOxygen += amount;
    }

    void RemoveOxygen(float amount)
    {
        currentOxygen -= amount;
    }

    void Attack() // Not currently called by anything, need input manager?
    {
        if (Time.time - lastAttackedTime >= attackRate)
        {
            RemoveOxygen(oxygenGunShotCost);
            Debug.Log("Unit attempted to attack");
            lastAttackedTime = Time.time;
        }
    }

}
