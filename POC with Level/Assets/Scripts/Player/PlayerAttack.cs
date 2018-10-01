using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    private Animator anim;
  
    public float resetAtkDuration;
    private int damage = 1;
    private float knockback = 1f;
    [HideInInspector]
    public int numOfAttack = 0;
    public float resetAtkTime;

    private Vector3 rawMousePos;//mouse stuff
    private Vector3 mousePos;

    public float minDashDistance = 2f;//how far away the cursor needs to be from the player to activate a dash, in world units
    public float maxDashDistance = 10f;//maximum distance the player can dash, in world units
    public float afterDashDelay ;//how long the player has to wait after a dash to resume walking again

    private Rigidbody2D rig2D;

    public float maxDashSpeed = 100f;//how fast the player can dash in DashToMouse()
    private float dashForce;//adds a little push at the end of the dash to simulate slowing down in DashToMouse()

    //public float maxNumberOfDashes = 4f;//how many times the player can dash in a row
    private float dashCounter = 0f;//counter for above

    Coroutine dashToMouse;//coroutine reference

    private void Start()
    {
        anim = GetComponent<Animator>();
        rig2D = GetComponent<Rigidbody2D>();
        dashForce = maxDashSpeed * 0.1f;
    }

    void resetTimer()
    {
        resetAtkTime += Time.deltaTime;
        if (resetAtkTime >= resetAtkDuration)
        {
            numOfAttack = 0;
            resetAtkTime = 0;
        }
    }

    void Attack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<EnemyCollision>().TakeDamage(damage);
            enemiesToDamage[i].GetComponent<EnemyCollision>().KnockBack(knockback);
        }
    }

    private void Update()
    {
        Vector3 rawMousePos2 = Input.mousePosition;
        Vector3 mousePos2 = Camera.main.ScreenToWorldPoint(rawMousePos2);

        if (Input.GetKeyDown(KeyCode.Mouse0) && numOfAttack < 4 && this.gameObject.GetComponent<slingShot>().isClicked == false && anim.GetBool("isInAir") == false 
            && anim.GetBool("isGettingHit") == false && this.gameObject.GetComponent<PlayerCollision>().hitSide == false)
        {
            numOfAttack += 1;
            resetAtkTime = 0;
            Attack();
            if (mousePos2.x > this.transform.position.x + 0.3f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (mousePos2.x < this.transform.position.x - 0.3f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            if(this.gameObject.GetComponent<PlayerCollision>().isAbleDash)
            {
                if (dashToMouse != null)
                {
                    StopCoroutine(dashToMouse);
                }
                dashToMouse = StartCoroutine(DashToMouse());
            }
        }

        //animator
        if (numOfAttack == 0)
        {
            anim.SetInteger("attackNum", 0);
        }
        else if (numOfAttack == 1)
        {
            anim.SetInteger("attackNum", 1);
            resetTimer();
        }
        else if (numOfAttack == 2)
        {
            anim.SetInteger("attackNum", 2);
            resetTimer();
        }
        else if (numOfAttack == 3)
        {
            anim.SetInteger("attackNum", 3);
            resetTimer();
        }
        else if (numOfAttack == 4)
        {
            anim.SetInteger("attackNum", 4);
            resetTimer();
        }
        if(numOfAttack == 0)
        {
            if (mousePos2.x > this.transform.position.x + 0.3f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (mousePos2.x < this.transform.position.x - 0.3f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }


    IEnumerator DashToMouse()
    {
        rawMousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(rawMousePos);

        Vector3 delta = mousePos - this.transform.position;

        mousePos.y = this.transform.position.y;
        mousePos.z = this.transform.position.z;

        if (Mathf.Abs(delta.x) > minDashDistance)
        {
            dashCounter += 1f;
            //state = State.Dash;//sets state to disable MouseFollow()
            float direction = delta.x;
            if (Mathf.Abs(delta.x) > maxDashDistance)
            {
                mousePos.x = this.transform.position.x + (Mathf.Sign(direction) * maxDashDistance);
            }
            while (Mathf.Abs(delta.x) > 1)//while the player has not reached the mouseclick location
            {
                //transform.position = Vector3.MoveTowards(transform.position, mousePos, maxDashSpeed * Time.deltaTime);
                rig2D.velocity = new Vector2(Mathf.Sign(direction) * maxDashSpeed, rig2D.velocity.y);
                delta = mousePos - this.transform.position;

                /*
                rigidbody2D.velocity = new Vector2(Mathf.Sign(delta.x) * maxDashSpeed, rigidbody2D.velocity.y);
                //rigidbody2D.AddForce(Vector2.right * (delta.x / Mathf.Abs(delta.x)) * dashForce);

                if (Mathf.Abs(rigidbody2D.velocity.x) > maxDashSpeed)
                    rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxDashSpeed, rigidbody2D.velocity.y);
                */
                //StopCoroutine(DashToMouse());

                yield return null;
            }
            rig2D.velocity = new Vector2(Mathf.Sign(direction) * dashForce, rig2D.velocity.y);
            yield return new WaitForSeconds(afterDashDelay);
            //state = State.Idle;
            dashCounter = 0f;
            StopCoroutine(dashToMouse);
        }

    }


}