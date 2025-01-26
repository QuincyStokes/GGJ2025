using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [Header("Floaty Settings")]
    public float initialPushStrength;


    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 randomForceDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1,1));
        rb.AddForce(randomForceDirection * initialPushStrength, ForceMode2D.Impulse);
    }

}
