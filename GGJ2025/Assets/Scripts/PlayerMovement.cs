using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    public Rigidbody2D rb;
    

    [Header("Movement Settings")]
    public float moveSpeed;

    private int currentRoomX;
    private int currentRoomY;

    

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
        Vector2 movement = new Vector3(horizontalInput, verticalInput).normalized;

        //Rigidbody approach
        //using add force uses acceleration, so will be sluggish movement
        //rb.AddForce(movement * moveSpeed, ForceMode2D.Force);

        //instead just directly set its velocity
        rb.velocity = movement * moveSpeed;


        //non rigidbody approach
        //transform.Translate(movement * moveSpeed);
    }
}



