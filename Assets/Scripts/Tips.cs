using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tips : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadGameAfterDelay());
    }

    IEnumerator LoadGameAfterDelay()
    {
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene("Parody");
    }
}
