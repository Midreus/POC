using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDotsCollision : MonoBehaviour {

    public GameObject player;
    public GameObject enemy;
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindWithTag("flyEnemy");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Collider2D>().isTrigger == false && other.gameObject.tag != "Player" && other.gameObject.tag != "flyEnemy")
        {

            player.GetComponent<slingShot>().collided(gameObject);

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Collider2D>().isTrigger == false)
        {

            player.GetComponent<slingShot>().uncollided(gameObject);

        }
    }
}
