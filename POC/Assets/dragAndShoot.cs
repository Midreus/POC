using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragAndShoot : MonoBehaviour
{

    Vector2 startPos, endPos, direction;

    [Range(0.05f, 1f)]
    public float throwForce = 0.05f;
    public float MAX_DISTANCE;
    private float distance;
    
    [HideInInspector]
    public Vector3 slingForce;
    public bool isClickedOnPlayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                if (hit.collider.gameObject.name == "Circle")
                {
                    startPos = Input.mousePosition;
                    isClickedOnPlayer = true;
                }
            }
        }
        if(Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            distance = Vector3.Distance(endPos, startPos);
            direction = startPos - endPos;
            if (distance > MAX_DISTANCE)
            {
                distance = MAX_DISTANCE;
            }
            slingForce = direction * distance * throwForce;
        }
        if (Input.GetMouseButtonUp(0) && isClickedOnPlayer == true)
        {
            GetComponent<Rigidbody2D>().AddForce(slingForce);
            isClickedOnPlayer = false;
        }
    }
}
