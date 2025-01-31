using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter2D(Collision2D other)
    {
        print($"Enemy projectile hit {other.gameObject.name}");
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerOxygen player = other.gameObject.GetComponent<PlayerOxygen>();
            player.ReduceOxygen(damage, true);
        }
        Destroy(gameObject);
    }
}
