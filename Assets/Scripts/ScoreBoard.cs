using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard Instance;
    public TMPro.TextMeshPro scoreText;
    public int gameScore;
    private string MenuName = "Menu";

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameScore = SharedData.score;
        scoreText.text = gameScore.ToString();
        SharedData.score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("return"))
        {
            ReturnToMenu();
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(MenuName);
    }

}
