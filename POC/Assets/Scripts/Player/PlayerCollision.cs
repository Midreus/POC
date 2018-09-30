using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D RB2D;
    private GameObject player;
    public GameObject groundSlamDust;
    private GameObject groundSlamDustHolder;
    private float dustDisappearTime;
    public bool hitSide = false;
    public float dropDelay;
    private float dropTime;

    //private float recoveryTime;
    //private float recoveryDelay = 1f;

    [HideInInspector]
    public bool isGrounded = true;
    public bool isOnPlatform = false;
    public bool isAbleDash;

    private void Start()
    {
        anim = GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        player = this.gameObject;
        isAbleDash = true;
    }

    void Update()
    {
        if(groundSlamDustHolder != null)
        {
            dustDisappearTime += Time.deltaTime;
            if(dustDisappearTime >= 0.2f)
            {
                Destroy(groundSlamDustHolder);
                dustDisappearTime = 0;
            }
        }
        if (RB2D.gravityScale == 0)
        {
            dropTime += Time.deltaTime;
            if (hitSide)
            {
                if (dropTime >= dropDelay)
                {
                    RB2D.gravityScale = 0.1f;
                    dropTime = 0;
                }
            }
            else
            {
                if (dropTime >= dropDelay)
                {
                    RB2D.gravityScale = 7;
                    dropTime = 0;
                }
            }
        }
        //if(RB2D.isKinematic )
        //{
        //    recoveryTime += Time.deltaTime;
        //    if(recoveryTime>=recoveryDelay)
        //    {
        //        RB2D.isKinematic = false;
        //        recoveryTime = 0;
        //    }
        //}
    }

    private void OnCollisionEnter2D(Collision2D other)
    {   
        if(other.gameObject.CompareTag("platform"))
        {
            isOnPlatform = true;
            foreach (ContactPoint2D hitPos in other.contacts)
            {
                if (GetComponent<GroundSlam>().isGroundSlam == false)
                {
                    /*
                    if (hitPos.normal.y < 0) // hit top
                    {
                        RB2D.velocity = new Vector2(0f, 0f);
                        RB2D.gravityScale = 0;
                        hitSide = false;
                    }
                    */
                    if (hitPos.normal.x > 0 && anim.GetBool("isInAir")) // hit left
                    {
                        RB2D.velocity = new Vector2(0f, 0f);
                        RB2D.gravityScale = 0;
                        hitSide = true;
                    }
                    else if (hitPos.normal.x < 0 && anim.GetBool("isInAir")) // hit right
                    {
                        RB2D.velocity = new Vector2(0f, 0f);
                        RB2D.gravityScale = 0;
                        hitSide = true;
                    }
                    else if(hitPos.normal.y > 0) // hit bottom
                    {
                        if(RB2D.gravityScale <7)
                        {
                            RB2D.gravityScale = 7;
                        }
                        hitSide = false;
                        isGrounded = true;
                    }
                }
                else
                {
                    if(hitPos.normal.y > 0) //hit bottom
                    {
                        player.GetComponent<GroundSlam>().isGroundSlam = false;
                        groundSlamDustHolder=Instantiate(groundSlamDust, transform.position, Quaternion.identity);
                        hitSide = false;
                        isGrounded = true;
                        //RB2D.isKinematic = true;
                    }
                }

                if(hitPos.normal.x != 0)
                {
                    //isAbleDash = false;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("platform"))
        {
            isOnPlatform = false;
            //isGrounded = false;
            if (RB2D.gravityScale <7)
            {
                RB2D.gravityScale = 7;
            }
            dropTime = 0;
           // isAbleDash = true;
        }
    }
}
