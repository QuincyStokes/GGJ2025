using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    public Rigidbody2D rb;
    public Transform playerSprite;
    

    [Header("Movement Settings")]
    public float moveSpeed;

    private int currentRoomX;
    private int currentRoomY;
    public AudioClip bumpedIntoSomething;
    public AudioMixerGroup SFXamg;

    

    void Start()
    {
        transform.position = new Vector3(GridManager.Instance.roomSizeX/2, GridManager.Instance.roomSizeY/2, 0);
    }
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }


    void Move()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if(horizontalInput < 0)
        {
            playerSprite.localScale = new Vector3(-10, 10, 1);
        }
        else
        {
            playerSprite.localScale = new Vector3(10, 10, 1);
        }
        Vector2 movement = new Vector3(horizontalInput, verticalInput);

        //Rigidbody approach
        //using add force uses acceleration, so will be sluggish movement
        //rb.AddForce(movement * moveSpeed, ForceMode2D.Force);

        //instead just directly set its velocity
        rb.velocity = movement * moveSpeed;


        //non rigidbody approach
        //transform.Translate(movement * moveSpeed);
    }

    public void StopMovement()
    {
        rb.velocity = new Vector3(0, 0, 0);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(!other.gameObject.CompareTag("Enemy"))
        {
            AudioManager.Instance.PlayOneShotVariedPitch(bumpedIntoSomething, .3f, SFXamg, .05f);
        }
    }
}



