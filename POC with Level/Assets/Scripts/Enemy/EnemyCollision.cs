using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

    private Rigidbody2D RB2D;
    private float height;
    private Animator anim;

    public int health;
    private Vector3 enemyPos;
    private GameObject enemyObject;
    public float Speed;
    private GameObject playerObject;
  //  private Vector2 knockBackForce;
    private float knockBackTime;
    private float knockBackDuration = 1f;
    private float airBorneTime;
    private float airBorneDuration = 3f;
    [HideInInspector]
    public bool isKnockBack = false;
    public bool isAirBorne = false;

    // Use this for initialization
    void Start () {
        RB2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyObject = this.gameObject;
        playerObject = GameObject.Find("Player");
    //    knockBackForce = new Vector2(5f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y > height + 3)
        {
            RB2D.velocity = new Vector2(0f, 0f);
            RB2D.gravityScale = 1;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (isKnockBack)
        {
            knockBackTime += Time.deltaTime;
            if(knockBackTime>= knockBackDuration)
            {
                knockBackTime = 0;
                isKnockBack = false;
                anim.SetBool("isGettingHit",false);
            }
        }
        if(isAirBorne)
        {
            airBorneTime += Time.deltaTime;
            if(airBorneTime >= airBorneDuration)
            {
                isAirBorne = false;
                airBorneTime = 0;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void KnockBack(float knockback)
    {
        enemyPos.y = enemyObject.transform.position.y;
        isKnockBack = true;
        anim.SetBool("isGettingHit",true);
        if (enemyObject.transform.position.x <= playerObject.transform.position.x)
        {
            //enemyPos.x = enemyObject.transform.position.x - knockback;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockback * Speed, 0));
            enemyObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (enemyObject.transform.position.x >= playerObject.transform.position.x)
        {
            //enemyPos.x = enemyObject.transform.position.x + knockback;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback * Speed, 0));
            enemyObject.transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
       // transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed * knockback);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("groundSlam") && anim.GetCurrentAnimatorStateInfo(0).IsName("normalWardenDown") == false && anim.GetCurrentAnimatorStateInfo(0).IsName("normalWardenGettingUp") == false)
        {
            height = transform.position.y;
            RB2D.gravityScale = 0;
            RB2D.velocity = new Vector2(0f, 20f);
            anim.SetBool("isAirBorne", true);
            isAirBorne = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("platform") && anim.GetCurrentAnimatorStateInfo(0).IsName("normalWardenDown") )
        {
            anim.SetBool("isAirBorne", false);
        }
    }
}
