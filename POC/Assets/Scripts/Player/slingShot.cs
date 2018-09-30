using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slingShot : MonoBehaviour
{
    public float initialDotSize;                //The intial size of the trajectoryDots gameobject
    public int numberOfDots;                    //The number of points representing the trajectory
    public float dotSeparation;                 //The space between the points representing the trajectory
    public float dotShift;                      //How far the first dot is from the "player"
    private GameObject trajectoryDots;          //The parent of all the points representing the trajectory
    private GameObject player;                  //The projectile the player will be shooting
    private Rigidbody2D playerRB;               //player's RigidBody2D
    private Vector3 playerPos;                  //Position of the player
    private Vector3 fingerPos;                  //Position of the pressed down finger/cursor on the screen 
    private Vector3 distanceDiff;             //The distance between where the finger/cursor is and where the "player" is when screen is being pressed
    private float x1, y1;						//X and Y position which will be applied to each point of the trajectory
    public bool isClicked2 = false;		    //If the finger/cursor is pressing down in the "Click Area" to activate the shot
    public float shootingPowerX;                //The amount of power which can be applied in the X direction
    public float shootingPowerY;				//The amount of power which can be applied in the Y direction
    public GameObject[] dots;					//The array of points that make up the trajectory
    //private GameObject clickArea;
    public bool mask;
    public float MaxStamina;
    private BoxCollider2D[] dotColliders;
    public Slider staminaBar;
    public float staminaRecoveryDelay;
    private float staminaRecoveryTime;
    public float staminaRecoverySpeed;
    private float currentStamina;
    public float staminaCost;

    private float clickTime;
    private float clickDuration = 0.1f;
    //  private GameObject flyEnemy;
    [HideInInspector]
    public Vector2 shotForce;					//How much velocity will be applied to the player
    public bool isClicked = false;             //If the cursor is hovering over the "Click Area"
                                               //public bool isGrounded = false;

    //  public bool hitFlyingEnemy = false;
    private Animator animator;
    

    // Use this for initialization
    void Start()
    {
        currentStamina = MaxStamina;
        player = this.gameObject;
        //clickArea = GameObject.Find("ClickArea");
        trajectoryDots = GameObject.Find("Trajectory Dots");
        playerRB = GetComponent<Rigidbody2D>();
        trajectoryDots.transform.localScale = new Vector3(initialDotSize, initialDotSize, trajectoryDots.transform.localScale.z); //Initial size of trajectoryDots is applied
        for (int k = 0; k < 10; k++)
        {
            dots[k] = GameObject.Find("dots (" + k + ")");           //All points are applied to the corresponding position in the dots array
        }
        for (int k = numberOfDots; k < 10; k++)
        {                   //If the number of points being used is less than 40, the maximum...
            GameObject.Find("dots (" + k + ")").SetActive(false);    //They will be hidden
        }
        trajectoryDots.SetActive(false);							//Trajectory initialization complete, the trajectory is hidden
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetBool("isGettingHit"))
        {
            trajectoryDots.SetActive(false);
            isClicked2 = false;
            this.gameObject.GetComponent<PlayerCollision>().isGrounded = true;
        }
        if (player.GetComponent<PlayerCollision>().isGrounded == false)
        {
            if (playerRB.velocity.x > 0 || shotForce.x > 2)
            {
                 //player.transform.rotation = Quaternion.Euler(0, 0, 0);
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (playerRB.velocity.x < 0 || shotForce.x < 2)
            {
                 //player.transform.rotation = Quaternion.Euler(0, 180f, 0);
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (currentStamina < MaxStamina)
        {
            staminaRecoveryTime += Time.deltaTime;
            if(GetComponent<PlayerCollision>().isOnPlatform == true && staminaRecoveryTime >= staminaRecoveryDelay)
            {
                currentStamina += Time.deltaTime * staminaRecoverySpeed;
            }
        }
        staminaBar.value = currentStamina / MaxStamina;
        if(GetComponent<PlayerCollision>().isOnPlatform == false)
        {
            staminaRecoveryTime = 0;
        }
        if(playerRB.velocity.y > -0.01 && playerRB.velocity.y < 0.01)
        {
            animator.SetBool("isInAir", false);
            animator.SetBool("isReadySling", false);
        }
        if (numberOfDots > 10)
        {
            numberOfDots = 10;
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && isClicked2 == false)
        {
            if (hit.collider.gameObject.name == player.gameObject.name)
            {
                isClicked = true;
            }
            else
            {
                isClicked = false;
            }
        }
        else
        {
            isClicked = false;
        }

        if (isClicked2 == true)
        {
            isClicked = true;
        }

        playerPos = player.transform.position;

        //if (shotForce.x > 0)
        //{
        //    //transform.rotation = Quaternion.Euler(0, 0, 0);
        //    this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        //}
        //else if (shotForce.x < 0)
        //{
        //  //  transform.rotation = Quaternion.Euler(0, 180f, 0);
        //    this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        //}

        if (Input.GetKey(KeyCode.Mouse0) && isClicked == true  && currentStamina>= staminaCost && animator.GetBool("isInAir") == false &&
            animator.GetBool("isGettingHit") == false) //|| hitFlyingEnemy == true && GetComponent<PlayerCollision>().isOnPlatform == true
        {   

            animator.SetBool("isWalking", false);
            fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    //The position of your finger/cursor is found
            fingerPos.z = 0;                                                    //The z position is set to 0
            clickTime += Time.deltaTime;
            
            if (Vector3.Distance(playerPos, fingerPos) < 5.5f)
            {
                distanceDiff = playerPos - fingerPos;
            }
            /*
            if(hitFlyingEnemy == true)
            {
                playerRB.velocity = new Vector2(0f, 0f);
                playerRB.isKinematic = true;
            }
            */
            
            if ((Mathf.Sqrt((distanceDiff.x * distanceDiff.x) + (distanceDiff.y * distanceDiff.y)) > (0.4f)) && clickTime >= clickDuration)
            { //If the distance between the finger/cursor and the "ball" is big enough...
                trajectoryDots.SetActive(true);                             //Display the trajectory
                animator.SetBool("isReadySling", true);
                isClicked2 = true;
                this.gameObject.GetComponent<PlayerCollision>().isGrounded = false;
            }
            else
            {
                trajectoryDots.SetActive(false);                                //Otherwise... Cancel the shot
                if (playerRB.isKinematic == true)
                {
                    playerRB.isKinematic = false;
                }
            }
            shotForce = new Vector2(distanceDiff.x * shootingPowerX, distanceDiff.y * shootingPowerY);	//The velocity of the shot is found
            for (int k = 0; k < numberOfDots; k++)
            {                           //Each point of the trajectory will be given its position
                x1 = playerPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * k + dotShift);    //X position for each point is found
                y1 = playerPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * k + dotShift) - (-Physics2D.gravity.y *7/ 2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (dotSeparation * k + dotShift) * (dotSeparation * k + dotShift));    //Y position for each point is found
                dots[k].transform.position = new Vector3(x1, y1, dots[k].transform.position.z); //Position is applied to each point
            }
        }
       
        
        if (Input.GetKeyUp(KeyCode.Mouse0) && isClicked2 == true && trajectoryDots.activeInHierarchy)
        {                               //If the player lets go...
            animator.SetBool("isReadySling", false);    
            isClicked2 = false;                                         //Aiming is no longer happening
            currentStamina -= staminaCost;
            if (trajectoryDots.activeInHierarchy)
            {                           //If the player was aiming...
                trajectoryDots.SetActive(false);                                //The trajectory will hide
                playerRB.velocity = new Vector2(shotForce.x, shotForce.y);    //The "ball" will have its new velocity
            }
            animator.SetBool("isInAir", true);
            
            if(playerRB.gravityScale == 0)
            {
                playerRB.gravityScale = 7;
            }
            /*
            if (hitFlyingEnemy == true)
            {
                flyEnemy.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-shotForce.x, -shotForce.y);
                hitFlyingEnemy = false;
                if(playerRB.isKinematic == true)
                {
                    playerRB.isKinematic = false;
                }   
            }
            */

            if (playerRB.isKinematic == true)
            {
                playerRB.isKinematic = false;
            }
            
        }
        
        //reset click
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            clickTime = 0;
        }
    }

    public void collided(GameObject dot)
    {
        for (int k = 0; k < numberOfDots; k++)
        {
            if (dot.name == "dots (" + k + ")")
            {
                for (int i = k + 1; i < numberOfDots; i++)
                {
                    dots[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }

    public void uncollided(GameObject dot)
    {
        for (int k = 0; k < numberOfDots; k++)
        {
            if (dot.name == "dots (" + k + ")")
            {

                for (int i = k - 1; i > 0; i--)
                {

                    if (dots[i].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                    {
                        return;
                    }
                }

                if (dots[k].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                {
                    for (int i = k; i > 0; i--)
                    {

                        dots[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;

                    }

                }
            }
        }
    }
  
    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if(other.gameObject.CompareTag("platform"))
    //    {
    //        isClicked2 = false;
    //    }
    //}
    /*
        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("flyEnemy") || other.gameObject.CompareTag("dots"))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && hitFlyingEnemy == false)
                {
                    flyEnemy = other.gameObject;
                    hitFlyingEnemy = true;
                    Debug.Log("HIT");
                }
                /*
                else if(Input.GetKeyUp(KeyCode.Mouse0))
                {
                    other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-shotForce.x, -shotForce.y);
                }

            }  
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("flyEnemy"))
            {
                hitFlyingEnemy = false;
            }
        }  
      */
}
