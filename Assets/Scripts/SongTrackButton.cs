using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackButtonUI : MonoBehaviour
{
    [SerializeField] private string TrackName = "Main";

    public void NewGame()
    {
        SceneManager.LoadScene(TrackName);
    }
}
