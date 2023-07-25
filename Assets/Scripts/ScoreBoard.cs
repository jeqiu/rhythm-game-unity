using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private string MenuName = "Menu";

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(MenuName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("return"))
        {
            ReturnToMenu();
        }
    }
}
