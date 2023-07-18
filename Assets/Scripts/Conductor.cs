using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Source")]
    public AudioClip countdown;

    // Start is called before the first frame update
    void Start()
    {
        sfxSource.clip = countdown;
        sfxSource.Play();
        //Debug.Log("Countdown Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
