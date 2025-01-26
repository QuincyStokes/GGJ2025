using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    //This is not complete at all
    public List<Enemy> enemyList;


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
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
