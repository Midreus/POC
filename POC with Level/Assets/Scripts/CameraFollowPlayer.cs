using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    public float xThreshold;
    public float yThreshold;
    private GameObject player;
    private Vector3 delta;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        delta = transform.position - player.transform.position;

        if (Mathf.Abs(delta.x) > xThreshold)
        {
            delta.x = xThreshold * Mathf.Sign(delta.x);
            transform.position = player.transform.position + delta;
        }
        if (Mathf.Abs(delta.y) > yThreshold)
        {
            delta.y = yThreshold * Mathf.Sign(delta.y);
            transform.position = player.transform.position + delta;
        }
    }
}
