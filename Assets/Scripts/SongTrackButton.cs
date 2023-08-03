using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackButtonUI : MonoBehaviour
{
    [SerializeField] private string TrackName1 = "Main";
    [SerializeField] private string TrackName2 = "Parody";
    [SerializeField] private string TrackName3 = "MainHSR";

    public void NewGame1()
    {
        SceneManager.LoadScene(TrackName1);
    }

    public void NewGame2()
    {
        SceneManager.LoadScene(TrackName2);
    }

    public void NewGame3()
    {
        SceneManager.LoadScene(TrackName3);
    }
}
