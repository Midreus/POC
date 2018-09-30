using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public float damage;
    public float knockback;

    //private bool allowMoving;
    //public float allowMovingTimer;
    //public float resetallowMovingtimer;

    private float attackTimer;
    private float maxAttackTimer = 2f;
    private float idleTimer = 0;
    private bool isIdle = false;
    private float playerDistance;
    private Vector2 enemyPos;
    private GameObject playerObject;
    private GameObject enemyObject;
    public float Speed;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    private Animator anim;
    private bool isAtk = false;

    private float readyTime;
    private float readyDuration = 0.5f;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyObject = this.gameObject;
        playerObject = GameObject.Find("Player");
    }

    public void FollowPlayer()
    {
        enemyPos.y = enemyObject.transform.position.y;

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("normalWardenWalking"))
        {
            if (enemyObject.transform.position.x < playerObject.transform.position.x)
            {
                enemyPos.x = playerObject.transform.position.x - 0.5f;
                enemyObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (enemyObject.transform.position.x > playerObject.transform.position.x)
            {
                enemyPos.x = playerObject.transform.position.x + 0.5f;
                enemyObject.transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
        }

        //if (Move == false)
        //    Move = true;

       
            transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed * Time.deltaTime);
    }

    public void Enemy_Attack()
    {       
        if(isAtk == false)
        {
            Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            for (int i = 0; i < playerToDamage.Length; i++)
            { 
                playerToDamage[i].GetComponent<PlayerHealth>().TakeDamage(damage);
                playerToDamage[i].GetComponent<PlayerHealth>().KnockBack(knockback);
                    //playerToDamage[i].GetComponent<PlayerController>().AllowMoving(allowMoving);
            } 
            isAtk = true;
        }
            
      
        //allowMoving = false;

        //if(isAttacking == true)
        //{
        //    timeBtwAtk = startTimeBtwAtk;
        //    isAttacking = false;
        //           // anim.Play(Attack_1);
        // }
    }
        
        //else
        //{
        //    Speed = 2f;
        //    idleTimer = maxIdleTimer;
        //    attackTimer = maxAttackTimer;
        //    //anim.SetBool("EnemyAttack", false);
        //    allowMoving = true;
        //}

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<EnemyCollision>().isKnockBack == false && this.gameObject.GetComponent<EnemyCollision>().isAirBorne == false && anim.GetCurrentAnimatorStateInfo(0).IsName("normalWardenAtkAntici") == false && isIdle == false)
        {
            playerDistance = Vector3.Distance(playerObject.transform.position, transform.position);
            if (Speed > 0)
            {
                FollowPlayer();
                anim.SetBool("isWalking", true);
            }
            if (playerDistance < 1f )
            {   
                Speed = 0;  //stop moving
                            //isAttacking = false
                readyTime = 0;
                idleTimer = 0;
                anim.SetBool("isWalking", false);
                anim.SetBool("isReadyHit", true);
                isAtk = false;
            }
            else
            {
                Speed = 2f; // continue to move
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("normalWardenAtkAntici"))
        {
            readyTime += Time.deltaTime;
            if(readyTime>= readyDuration)
            {
                anim.SetBool("isReadyHit",false);
                Enemy_Attack();
                isIdle = true;
            }
        }

        if (isIdle == true)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > maxAttackTimer)
            {
                isIdle = false;
            }
        }
        //if(PlayerHealth.isAlive == false)
        //{
        //    anim.Play(Idle);
        //}

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
