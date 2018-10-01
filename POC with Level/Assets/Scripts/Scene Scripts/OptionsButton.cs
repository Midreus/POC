using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour {
    
    public GameObject parentObject;
    public GameObject targetObject;
    public GameObject targetButton;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void Options()
    {
        StartCoroutine(OptionsCoroutine());


    }

    IEnumerator OptionsCoroutine()
    {
        targetObject.SetActive(true);
        targetButton.GetComponent<BackButton>().targetObject = parentObject;
        parentObject.SetActive(false);
        yield return null;


    }
}
