using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public AudioSource beatSFX;
    public AudioSource perfectSFX;
    public TMPro.TextMeshPro scoreText;

    void Start()
    {
        Instance = this;
        SharedData.score = 0;
        SharedData.maxScore = 0;
    }
    public static void Hit()
    {
        SharedData.score += 1;
        Instance.hitSFX.Play();
    }
    public static void PerfectHit()
    {
        SharedData.score += 2;
        Instance.perfectSFX.Play();
    }
    public static void Miss()
    {
        Instance.missSFX.Play();
    }
    public static void Beat()
    {
        Instance.beatSFX.Play();
    }
    private void Update()
    {
        scoreText.text = (SharedData.score).ToString();
    }
}