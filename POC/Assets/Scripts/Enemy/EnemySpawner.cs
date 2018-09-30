using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnTime;
    //The amount of time between each spawn.
    public float spawnDelay = 1f;
    //The amount of time before spawning starts.
    public GameObject[] enemies;
    //Array of enemy prefabs.
    public float SpawnLimit;
    private bool isSpawn = true;
    GameObject holder;
    
    public  List<GameObject> SpawnList = new List<GameObject>();

    void Start()
    {
        //Start calling the Spawn function repeatedly after a delay.
      //  InvokeRepeating("Spawn", spawnDelay, spawnTime);
    }

    private void Update()
    {   
        for(int i = 0;i<SpawnList.Count;i++)
        {
            if (SpawnList[i] == null)
            {
                SpawnList.RemoveAt(i);
                //Destroy(SpawnList[i].gameObject);
            }
        }
        if(isSpawn)
        {
            spawnTime += Time.deltaTime;
            if(spawnTime >= spawnDelay)
            {
                Spawn();
                spawnTime = 0;
            }
           
        }
    }

    void Spawn()
    { 
        if(SpawnList.Count >= SpawnLimit)
        {
            isSpawn = false;
        }
        else
        {
            //Instantiate a random enemy.
            int enemyIndex = Random.Range(0, enemies.Length);
            holder = (GameObject)Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
            SpawnList.Add(holder);
        }
    }
}
