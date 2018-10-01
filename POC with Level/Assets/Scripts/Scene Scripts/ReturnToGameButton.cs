using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGameButton : MonoBehaviour {
    public GameObject gameObject;

    public void ReturnToGame()
    {
        StartCoroutine(ReturnToGameCoroutine());


    }

    IEnumerator ReturnToGameCoroutine()
    {
        float timeScale = transform.parent.parent.GetComponent<PauseController>().timeScale;
        Time.timeScale = timeScale;
        gameObject.SetActive(false);
        yield return null;


    }
}
