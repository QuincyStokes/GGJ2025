using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //
    [SerializeField] int maxNumberOfSpawns = 5;
    [SerializeField] int minNumberOfSpawns = 1;


//  QUINCY ADDED THIS LINE BELOW
    public static EnemySpawnManager Instance;

    //This is not complete at all
    public List<Enemy> enemyList;
    public List<Enemy> spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {

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
        int numberOfEnemiesToSpawn = Random.Range(minNumberOfSpawns, maxNumberOfSpawns);
        Debug.Log("Number of Enemies to Spawn:" + numberOfEnemiesToSpawn);
        int numberOfEnemyTypes = enemyList.Count;
        int enemiesSpawned = 0; //QUINCY TRIED HERE, and set while (enemiesSpawned <)

        while (enemiesSpawned < numberOfEnemiesToSpawn)              // assuming player has to clear the room of enemies
        {
            int EnemyToSpawn = Random.Range(0, numberOfEnemyTypes - 1);         //randomly chooses an enemy type to spawn
            Enemy e = enemyList[EnemyToSpawn];  
            Enemy f = Instantiate(e);
            spawnedEnemies.Add(f);
            enemiesSpawned++; //QUINCY TRIED HERE          
        }
    }

}
