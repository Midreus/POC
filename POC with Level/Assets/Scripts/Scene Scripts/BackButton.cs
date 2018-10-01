using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

    public GameObject parentObject;
    public GameObject targetObject;
    public int sceneIndex = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    public void Back()
    {
        StartCoroutine(BackCoroutine());


    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator BackCoroutine()
    {
        Debug.Log("running");
        targetObject.SetActive(true);
        parentObject.SetActive(false);
        yield return null;


    }
}
