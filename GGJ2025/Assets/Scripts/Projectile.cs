using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter2D(Collision2D other)
    {
        print($"Player's projectile damaging {other.gameObject.name}.");
        if(other.gameObject.CompareTag("Enemy"))
        {
            
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
