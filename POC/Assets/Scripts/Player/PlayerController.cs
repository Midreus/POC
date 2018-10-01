using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float threshold = 1f;//how far away the mouse has to move from the player to trigger MouseFollow(), in world units
    public float moveForce = 365f;// how much force is applied on the rigidbody in MouseFollow()
    public float maxSpeed = 10f;//max speed of the rigidbody in MouseFollow()
    
    public float maxDashSpeed = 100f;//how fast the player can dash in DashToMouse()
  //  private float dashForce;//adds a little push at the end of the dash to simulate slowing down in DashToMouse()
    
    public float minDashDistance = 2f;//how far away the cursor needs to be from the player to activate a dash, in world units
    public float maxDashDistance = 10f;//maximum distance the player can dash, in world units
  //  public float afterDashDelay = 1f;//how long the player has to wait after a dash to resume walking again

    public float maxNumberOfDashes = 4f;//how many times the player can dash in a row
  //  private float dashCounter = 0f;//counter for above
    private bool stopMoving;
    

    private Vector3 rawMousePos;//mouse stuff
    private Vector3 mousePos;
    public GameObject PlayerObject;
    private Rigidbody2D rig2D;//allows modifications to rigidbody's velocity

    private Animator anim;

   // Coroutine dashToMouse;//coroutine reference
    public Component playerAttack;

    public enum State//placeholder for animation states
    {
        Idle,
        Dash,
        TotalStates
    }
    public State state;//calling the enum

    void Awake()
	{
        rig2D = GetComponent<Rigidbody2D>();//sets reference to the rigidbody attached to player
    //    dashForce = maxDashSpeed/2;//seemed like a nice value
        state = State.Idle;//initial state
        playerAttack = this.gameObject.GetComponent<PlayerAttack>();
    }

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        //Debug.Log ("Starting");
    }

	// Update is called once per frame
	void Update ()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        if(this.gameObject.GetComponent<PlayerCollision>().hitSide == false && anim.GetBool("isReadySling") == false && anim.GetBool("isInAir") == false && this.gameObject.GetComponent<slingShot>().isClicked == false && anim.GetBool("isGettingHit") == false)
        {
            //if (Input.GetButtonDown("Fire1") && this.gameObject.GetComponent<slingShot>().isClicked == false)//defaults to left mouse button
            //{
            //    if (dashCounter < maxNumberOfDashes)
            //    {
            //        //stops and restarts the dash coroutine if maximum number of dashes is not reached yet
            //        if (dashToMouse != null)
            //        {
            //            StopCoroutine(dashToMouse);
            //        }
            //        //dashToMouse = StartCoroutine(DashToMouse());
            //    }

            //}
            if (this.gameObject.GetComponent<PlayerAttack>().resetAtkTime == 0)//(state == State.Idle)
            {
                MouseFollow();
            }
            /*if (mousePos.x > PlayerObject.transform.position.x + 0.3f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (mousePos.x < PlayerObject.transform.position.x - 0.3f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }*/
        }
    }

   /* public void AllowMoving(bool allowMoving)
    {
        if(allowMoving == false)
        {
            //StopCoroutine(DashToMouse());
            moveForce = 0f;
            maxSpeed = 0f;
        }
        else
        {
            //StartCoroutine(DashToMouse());
            stopMoving = false;
            moveForce = 150f;
            maxSpeed = 1f;
        }

    }*/

    void MouseFollow()
    {
        rawMousePos = Input.mousePosition;

        mousePos = Camera.main.ScreenToWorldPoint(rawMousePos);

        Vector3 delta = mousePos - this.transform.position;

        moveForce = 150f;
        maxSpeed = 1f;

        if (Mathf.Abs(delta.x) > threshold)
        {
            anim.SetBool("isWalking", true);
            //anim.Play(walking);
            rig2D.AddForce(Vector2.right * (delta.x / Mathf.Abs(delta.x)) * (moveForce * Time.deltaTime*60));
        }
        else if (Mathf.Abs(delta.x) < threshold)
        {
            anim.SetBool("isWalking", false);
            //anim.Play(idle);
        }

        if (Mathf.Abs(rig2D.velocity.x) > maxSpeed)
        {
            rig2D.velocity = new Vector2(Mathf.Sign(rig2D.velocity.x) * maxSpeed, rig2D.velocity.y);
        }
    }

    

    //IEnumerator DashToMouse()
    //{
    //    rawMousePos = Input.mousePosition;
    //    mousePos = Camera.main.ScreenToWorldPoint(rawMousePos);

    //    Vector3 delta = mousePos - this.transform.position;

    //    mousePos.y = this.transform.position.y;
    //    mousePos.z = this.transform.position.z;

    //    if (Mathf.Abs(delta.x) > minDashDistance)
    //    {
    //        dashCounter += 1f;
    //        state = State.Dash;//sets state to disable MouseFollow()
    //        float direction = delta.x;
    //        if (Mathf.Abs(delta.x) > maxDashDistance)
    //        {
    //            mousePos.x = this.transform.position.x + (Mathf.Sign(direction)*maxDashDistance);
    //        }
    //        while (Mathf.Abs(delta.x) > 1)//while the player has not reached the mouseclick location
    //        {
    //            transform.position = Vector3.MoveTowards(transform.position, mousePos, maxDashSpeed * Time.deltaTime);

    //            delta = mousePos - this.transform.position;

    //            /*
    //            rigidbody2D.velocity = new Vector2(Mathf.Sign(delta.x) * maxDashSpeed, rigidbody2D.velocity.y);
    //            //rigidbody2D.AddForce(Vector2.right * (delta.x / Mathf.Abs(delta.x)) * dashForce);

    //            if (Mathf.Abs(rigidbody2D.velocity.x) > maxDashSpeed)
    //                rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxDashSpeed, rigidbody2D.velocity.y);
    //            */
    //            //StopCoroutine(DashToMouse());

    //            yield return null;
    //        }
    //        rig2D.velocity = new Vector2(Mathf.Sign(direction) * dashForce, rig2D.velocity.y);
    //        yield return new WaitForSeconds(afterDashDelay);
    //        state = State.Idle;
    //        dashCounter = 0f;
    //        StopCoroutine(dashToMouse);
    //    }

        
        
    //}
}