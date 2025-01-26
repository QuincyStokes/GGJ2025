using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //
    [SerializeField] int maxNumberOfSpawns = 5;
    [SerializeField] int maxNumberOfUniqueSpawns = 3;
    [SerializeField] int minNumberOfSpawns = 1;


//  QUINCY ADDED THIS LINE BELOW
    public static EnemySpawnManager Instance;

    //This is not complete at all
    public List<Enemy> enemyList;
    public Dictionary<int,int> spawnedEnemies;            // using a Dictionary to keep track of how many of each enemy are present Key = Enemy, Value = How many of them are alive

    //@@@@@@@@@@ TO DO: Update the dictionary whenever an enemy DIES. Currently, combat hasn't been implemented yet. Code will NOT work once units die.

    // Start is called before the first frame update
    void Start()
    {
        if (enemyList.Count != 0)
        {
            foreach (Enemy e in enemyList)
            {
                Instantiate(e);
                Debug.Log("Enemy spawned");
            }
        }
        //QUINCY ADDING THIS LINE
        spawnedEnemies = new Dictionary<int, int>();

    }
   //QUINCY ADDING AWAKE FUNCTION AND EVERYTHING INSIDE
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies()
    {
        int numberOfEnemiesToSpawn = Random.Range(minNumberOfSpawns, maxNumberOfUniqueSpawns);
        int numberOfEnemyTypes = enemyList.Count;
        //int enemiesSpawned = 0; QUINCY TRIED HERE, and set while (enemiesSpawned <)

        while (spawnedEnemies.Count <= numberOfEnemiesToSpawn)              // assuming player has to clear the room of enemies
        {
            int EnemyToSpawn = Random.Range(0, numberOfEnemyTypes);         //randomly chooses an enemy type to spawn

            if (spawnedEnemies.ContainsKey(EnemyToSpawn))                   //Checks if our dict already has records of this enemy being spawned
            {
                if (spawnedEnemies[EnemyToSpawn] == maxNumberOfUniqueSpawns) //If we have the max number of unique spawns already, skip spawning that one and reroll.
                    continue;                                                //Otherwise, spawn an enemy.
                else
                {
                    Instantiate(enemyList[EnemyToSpawn]);
                    spawnedEnemies[EnemyToSpawn] += 1;                      // Adds to our count of enemies that have spawned
                    //enemiesSpawned++; QUINCY TRIED HERE
                }
            }

            else
            {
                Instantiate(enemyList[EnemyToSpawn]);            //Adds the Key/Value pair to our dictionary after instantiating the enemy object.
                spawnedEnemies.Add(EnemyToSpawn, 1);
                //enemiesSpawned++; QUINCY TRIED HERE
            }
           
        }
    }

}
