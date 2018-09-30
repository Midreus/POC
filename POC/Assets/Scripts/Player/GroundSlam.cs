using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : MonoBehaviour {
    public float groundSlamDelay;
    private float groundSlamTime;
    public float groundSlamForce;
    private Animator anim;
    [HideInInspector]
    public bool isGroundSlam = false;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponent<PlayerCollision>().isGrounded == false && GetComponent<PlayerCollision>().isOnPlatform == false && Input.GetKey(KeyCode.Mouse0) &&anim.GetBool("isInAir"))
        {
            groundSlamTime += Time.deltaTime;
            if(groundSlamTime>= groundSlamDelay)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -groundSlamForce);
                isGroundSlam = true;
                groundSlamTime = 0;
            }
        }
	}
}
