using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackButtonUI : MonoBehaviour
{
    [SerializeField] private string GameInfo = "GameInfo";
    [SerializeField] private string TrackName1 = "Main";
    [SerializeField] private string TrackName2 = "Parody";
    [SerializeField] private string TrackName3 = "MainHSR";

    public void NewGame1()
    {
        SharedData.trackName = TrackName1;
        SceneManager.LoadScene(GameInfo);
    }

    public void NewGame2()
    {
        SharedData.trackName = TrackName2;
        SceneManager.LoadScene(GameInfo);
    }

    public void NewGame3()
    {
        SharedData.trackName = TrackName3;
        SceneManager.LoadScene(GameInfo);
    }

}

