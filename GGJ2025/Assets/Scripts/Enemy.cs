using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Unit Values to Set
    [SerializeField] private float speed = 1.0f;                        // How fast the unit moves 
    [SerializeField] private float meleeDistance = 0.5f;                // How close the unit needs to be in order to attempt a melee attack
    [SerializeField] private float rangeDistance = 5.0f;                // How close the unit needs to be in order to attempt a ranged attack
    [SerializeField] private float unitPreferredDistance = 5.0f;        // How far the unit wants to stand in relation to the player.
    [SerializeField] private float attackRate = 1.0f;                   // How often the unit can attack per seconds
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private float projectileSpeed;

    //Component Info to keep track of
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Enemy enemyPrefab;
    

    //Internal Info for the Unit to keep track of
    private float lastAttackedTime = 0f;
    private float currentHealth;

    //Room info for the Unit to keep track of
    private float roomX1;
    private float roomX2;
    private float roomY1;
    private float roomY2;

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
        GameObject playerObject = GameObject.FindWithTag("Player");         // We need to keep track of the player in order to determine movement, so we use this to find out the player's positions before starting anything.
        if (playerObject != null)
        {
            playerTransform = playerObject.GetComponent<Transform>();
        }

        UpdateXY();

        float randomX = Random.Range(roomX1, roomX2);
        float randomY = Random.Range(roomY1, roomY2);

        transform.position = new Vector3(randomX, randomY, 0); // @@@@@@@@@ TO DO: Needs to be randomized, currently hard coded
        currentHealth = maxHealth;
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Move();
    }

    void Move()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        var step = speed * Time.deltaTime;

        if ((enemyType & Options.Follow) == Options.Follow)                             // If the unit is a "Follow" type: it will always move towards the player's current position.
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, step);
        }

        if ((enemyType & Options.Kite) == Options.Kite)                                 // If the unit is a "Kite" type: it will always try to stay in range of their attack range, but away from the player.
        {
            if (distance != unitPreferredDistance)
            {
                if (distance < unitPreferredDistance)
                    rb.velocity = direction * speed;
                else
                    transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, step);
            }     
        }

        if ((enemyType & Options.Melee) == Options.Melee)                               // If the unit is a "Melee" type: they will only attempted to attack at close range
        {
            if (distance <= meleeDistance)
            {
                Attack();
            }
        }

        if ((enemyType & Options.Ranged) == Options.Ranged)                             // If the unit is a "Ranged" type: they will attempt to attack at minimum range
        {
            if (distance <= rangeDistance)
            {
                Attack();
            }
        }
    }

    void UpdateXY()
    {
        roomX1 = GridManager.Instance.currentCamX * GridManager.Instance.roomSizeX;
        roomX2 = GridManager.Instance.currentCamX * GridManager.Instance.roomSizeX + GridManager.Instance.roomSizeX;
        roomY1 = GridManager.Instance.currentCamY * GridManager.Instance.roomSizeY;
        roomY2 = GridManager.Instance.currentCamY * GridManager.Instance.roomSizeY + GridManager.Instance.roomSizeY;
    }

    //@@@@@@@@ TO DO: Implement attacks for the enemy. Probably just spawn some effects
    void Attack()
    {
        if (Time.time - lastAttackedTime >= attackRate)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector3 playerPosition = playerTransform.position; 
           
            Vector3 fireDirection = (playerPosition - transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = fireDirection * projectileSpeed;
        }
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
