using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallConfirmationMenuButton : MonoBehaviour {

    public GameObject parentObject;
    public GameObject targetObject;
    public GameObject confirmObject;
    public GameObject confirmButton;
    public GameObject denyButton;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void Confirmation()
    {
        StartCoroutine(ConfirmationCoroutine());


    }

    IEnumerator ConfirmationCoroutine()
    {
        targetObject.SetActive(true);
        denyButton.GetComponent<BackButton>().targetObject = parentObject;
        confirmButton.GetComponent<BackButton>().targetObject = confirmObject;
        parentObject.SetActive(false);
        yield return null;


    }
}
