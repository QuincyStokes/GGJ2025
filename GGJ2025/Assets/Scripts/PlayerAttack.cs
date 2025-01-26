
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAttack : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject projectile; //this will be bubble(s)
    public float fireSpeed;
    public float shootCooldown;
    private float currentCooldownTime;
    public List<AudioClip> attackBarks;
    public AudioMixerGroup SFXamg;
    public PlayerOxygen playerOxygen;
    public float oxygenShootAmount;

    // Start is called before the first frame update

    void Start()
    {
        currentCooldownTime = 0f;
        playerOxygen = GetComponent<PlayerOxygen>();
    }
    void Update()
    {
        currentCooldownTime += Time.deltaTime;
        if(Input.GetKeyDown(InputHandler.Instance.playerShoot))
        {
            if(currentCooldownTime > shootCooldown)
            {
                currentCooldownTime = 0f;
                //shoot
                //spawn a projetile, give it a velocity in the direction of the players mouse
                print("pew");
                GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0f;
                Vector3 fireDirection = (mousePos - transform.position).normalized;
                newProjectile.GetComponent<Rigidbody2D>().velocity = fireDirection * fireSpeed;
                playerOxygen.ReduceOxygen(oxygenShootAmount);
                AudioManager.Instance.PlayOneShotVariedPitch(attackBarks[Random.Range(0, attackBarks.Count())], .3f, SFXamg, .05f);

            }
            
        }
    }
}
