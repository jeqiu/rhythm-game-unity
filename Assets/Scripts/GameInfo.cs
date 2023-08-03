using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadGameAfterDelay());
    }

    IEnumerator LoadGameAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
    }

}
