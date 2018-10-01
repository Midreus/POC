using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour {
    public Animator anim;
    public float waitFor;
    public GameObject gameObject;
    public int sceneIndex;
    
	public void StartGame () {
        StartCoroutine(StartGameCoroutine());
        
        
    }

    IEnumerator StartGameCoroutine()
    {
        anim.SetTrigger("Fade Out");
        yield return new WaitForSecondsRealtime(waitFor);
        anim.SetTrigger("Fade In");
        SceneManager.LoadScene(sceneIndex);
        //Time.timeScale = 1f;
        //gameObject.SetActive(false);
        
        
    }
    
}
