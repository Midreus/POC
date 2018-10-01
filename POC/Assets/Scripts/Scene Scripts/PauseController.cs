using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {
    public GameObject pauseMenu;
    public float timeScale;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Cancel") && Time.timeScale>0f)
        {
            timeScale = Time.timeScale;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
	}
}
