using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OxygenBubble : MonoBehaviour
{
    [Header("Oxygen Amount")]
    public float oxygenAmount;
    public List<AudioClip> bubblePops;
    public AudioMixerGroup SFXamg;
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerOxygen>().IncreaseOxygen(oxygenAmount);
            AudioManager.Instance.PlayOneShot(bubblePops[Random.Range(0, bubblePops.Count)], .3f, SFXamg);
            Destroy(gameObject);
        }

    }
}
