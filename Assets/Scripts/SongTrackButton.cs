using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackButtonUI : MonoBehaviour
{
    [SerializeField] private string GameInfo = "GameInfo";
    [SerializeField] private string TrackName1 = "Main";
    [SerializeField] private string TrackName2 = "Tips";
    [SerializeField] private string TrackName3 = "MainHSR";

    public void NewGame1()
    {
        SceneManager.LoadScene(TrackName1);
    }

    public void NewGame2()
    {
        SceneManager.LoadScene("Tips");
    }

    public void NewGame3()
    {
        SceneManager.LoadScene(TrackName3);
    }

    public void ShowGameInfo()
    {
        SceneManager.LoadScene(GameInfo);
    }

}

