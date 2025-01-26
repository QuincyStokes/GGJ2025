using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Unit Values to Set
    [SerializeField] private float speed = 1.0f;                        // How fast the unit moves 
    [SerializeField] private float meleeDistance = 0.5f;                // How close the unit needs to be in order to attempt a melee attack
    [SerializeField] private float rangeDistance = 5.0f;                // How close the unit needs to be in order to attempt a ranged attack
    [SerializeField] private float unitPreferredDistance = 5.0f;        // How far the unit wants to stand in relation to the player.
    [SerializeField] private float attackRate = 1.0f;                   // How often the unit can attack per seconds

    //Component Info to keep track of
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private GridManager gridManager;

    //Internal Info for the Unit to keep track of
    private float lastAttackedTime = 0f;

    // Flags for Determining Enemy Movement Type / Attack Behavior
    [System.Flags]
    public enum Options
    {
        None = 1 << 0,
        Melee = 1 << 1,         //Enemy needs close range / collision before attacking
        Ranged = 1 << 2,        //Enemy can attack from longer range
        Follow = 1 << 3,        // Enemy will try to move towards the player
        Kite = 1 << 4,          //Enemy will try to stay away from the player
        Stay = 1 << 5           //Enemy will stand in one place
    }

    public Options enemyType;   //Allows us to determine the enemy type in the inspector


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(GridManager.Instance.roomSizeX / 5, GridManager.Instance.roomSizeY / 5, 0);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Spawn()
    {
        
    }

    void Move()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        var step = speed * Time.deltaTime;

        if ((enemyType & Options.Follow) == Options.Follow)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, step);
        }

        if ((enemyType & Options.Kite) == Options.Kite)
        {
            if (distance != unitPreferredDistance)
            {
                if (distance < unitPreferredDistance)
                    rb.velocity = direction * speed;
                else
                    transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, step);

            }

                
                
        }

        if ((enemyType & Options.Melee) == Options.Melee)
        {
            if (distance <= meleeDistance)
            {
                Attack();
            }
        }

        if ((enemyType & Options.Ranged) == Options.Ranged)
        {
            if (distance <= rangeDistance)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackedTime >= attackRate)
        {
            Debug.Log("Unit attempted to attack");
            lastAttackedTime = Time.time;
        }
    }
}
