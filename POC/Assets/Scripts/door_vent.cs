using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_vent : MonoBehaviour {

    private GameObject player;
    private Animator playerAnim;
    private GameObject spawner;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        playerAnim = player.GetComponent<Animator>();
        spawner = GameObject.Find("enemySpawner");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D other)
    {   
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("spinning") && other.gameObject.CompareTag("Player") && spawner.GetComponent<EnemySpawner>().SpawnList.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
