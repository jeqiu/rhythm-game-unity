using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SfxSource;

    [Header("Audio Source")]
    public AudioClip Countdown;
    public AudioClip SongMusic;

    // Start is called before the first frame update
    void Start()
    {
        SfxSource.clip = Countdown;
        SfxSource.PlayScheduled(AudioSettings.dspTime);
        Debug.Log("Countdown Started" + " at dspTime " + AudioSettings.dspTime);

        MusicSource.clip = SongMusic;
        MusicSource.PlayScheduled(AudioSettings.dspTime + 5);
        Debug.Log("Song will start 5 seconds" + " from dspTime " + AudioSettings.dspTime);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
