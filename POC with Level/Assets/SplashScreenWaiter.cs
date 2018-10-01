using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenWaiter : MonoBehaviour {

    public float secondsToWait;
    public int sceneIndex = 1;

	// Use this for initialization
	void Start () {
        StartCoroutine("WaitForSplashScreens");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WaitForSplashScreens()
    {
        Debug.Log("running");
        yield return new WaitForSeconds(secondsToWait);
        SceneManager.LoadScene(sceneIndex);
    }
}
