using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public static float health;
    public float maxHealth;
    private Vector2 playerPos;
    public GameObject enemyObject;
    public float Speed;
    public GameObject playerObject;
    public static bool isAlive = true;

    private float immuneTimer;
    private float immuneDuration = 1f;
    private bool isImmune;
 //   public float Damage;
  //  public float maxDamage;
  //  public float PushBack;
 //   public float maxPushBack;

    public Animator anim;
    public string knockback_animation;
    public string idle_animation;

    public Slider healthbar;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        health = maxHealth;
        //isAlive = true;
        healthbar.value = CalculateHealth();
	}
	
	// Update is called once per frame
	void Update () {
        if(isImmune == true)
        {
            immuneTimer += Time.deltaTime;
            if(immuneTimer >= immuneDuration)
            {
                isImmune = false;
                immuneTimer = 0;
                anim.SetBool("isGettingHit", false);
            }
        }

        if (health <= 0)
        {
            //anim.SetBool("EnemyAttack", false);
            Destroy(gameObject);
            //isAlive = false;
        }
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if(isImmune == false)
        {
            health -= damage;
            healthbar.value = CalculateHealth();
        }
    }
    
    public void KnockBack(float knockback)
    {
        if(isImmune == false)
        {
            anim.SetBool("isGettingHit", true);
            playerPos.y = playerObject.transform.position.y;

            if (enemyObject.transform.position.x <= playerObject.transform.position.x)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback * Speed, 0));
                //playerObject.transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
            else if (enemyObject.transform.position.x >= playerObject.transform.position.x)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockback * Speed, 0));
                //playerObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            isImmune = true;
        }
        // PushBack = knockback;
        
    }

}
