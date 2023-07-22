using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public AudioSource beatSFX;
    public TMPro.TextMeshPro scoreText;
    static int score;
    void Start()
    {
        Instance = this;
        score = 0;
    }
    public static void Hit()
    {
        score += 1;
        Instance.hitSFX.Play();
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
        scoreText.text = score.ToString();
    }
}