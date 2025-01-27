using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenBubble : MonoBehaviour
{
    [Header("Oxygen Amount")]
    public float oxygenAmount;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerOxygen>().IncreaseOxygen(oxygenAmount);
            Destroy(gameObject);
        }

    }
}
