using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongPreview : MonoBehaviour
{
    public float PreviewLength = 20;
    public float PauseLength = 0.2f;

    [Header("Audio Source")]
    [SerializeField] AudioSource PreviewSource1;
    [SerializeField] AudioSource PreviewSource2;
    [SerializeField] AudioSource PreviewSource3;
    public AudioClip Preview;
    public float StartVolume1 = -1;
    public float StartVolume2 = -1;
    public float StartVolume3 = -1;

    public void StartPreview1()
    {
        if (StartVolume2 != -1) { EndPreview2(); }
        if (StartVolume3 != -1) { EndPreview3(); }
        PreviewSource1 = GetComponents<AudioSource>()[0];
        PreviewSource1.volume = SharedData.musicVolume;
        StartVolume1 = PreviewSource1.volume;
        Debug.Log("Starting Volume " + PreviewSource1.volume);
        StartCoroutine(PlaySongPreview1());
    }

    public void StartPreview2()
    {
        if (StartVolume1 != -1) { EndPreview1(); }
        if (StartVolume3 != -1) { EndPreview3(); }
        PreviewSource2 = GetComponents<AudioSource>()[1];
        PreviewSource2.volume = SharedData.musicVolume;
        StartVolume2 = PreviewSource2.volume;
        Debug.Log("Starting Volume " + PreviewSource2.volume);
        StartCoroutine(PlaySongPreview2());
    }

    public void StartPreview3()
    {
        if (StartVolume1 != -1) { EndPreview1(); }
        if (StartVolume2 != -1) { EndPreview2(); }
        //PreviewSource.clip = Preview;
        PreviewSource3 = GetComponents<AudioSource>()[2];
        PreviewSource3.volume = SharedData.musicVolume;
        StartVolume3 = PreviewSource3.volume;
        Debug.Log("Starting Volume " + PreviewSource3.volume);
        StartCoroutine(PlaySongPreview3());
    }

    IEnumerator PlaySongPreview1()
    {
        float CurrentTime = 0;
        PreviewSource1.Play();
        yield return new WaitForSeconds(15);
        while (CurrentTime < PreviewLength)
        {
            CurrentTime += Time.deltaTime;
            PreviewSource1.volume = Mathf.Lerp(StartVolume1, 0, CurrentTime / PreviewLength);
            Debug.Log("Volume " + PreviewSource1.volume);
            yield return null;
        }
        Debug.Log("end preview at " + AudioSettings.dspTime);
        EndPreview1();
        yield break;
    }

    IEnumerator PlaySongPreview2()
    {
        float CurrentTime = 0;
        PreviewSource2.time = 5;
        PreviewSource2.Play();
        yield return new WaitForSeconds(15);
        while (CurrentTime < PreviewLength)
        {
            CurrentTime += Time.deltaTime;
            PreviewSource2.volume = Mathf.Lerp(StartVolume2, 0, CurrentTime / PreviewLength);
            Debug.Log("Volume " + PreviewSource2.volume);
            yield return null;
        }
        Debug.Log("end preview at " + AudioSettings.dspTime);
        EndPreview2();
        yield break;
    }

    IEnumerator PlaySongPreview3()
    {
        float CurrentTime = 0;
        PreviewSource3.Play();
        yield return new WaitForSeconds(15);
        while (CurrentTime < PreviewLength)
        {
            CurrentTime += Time.deltaTime;
            PreviewSource3.volume = Mathf.Lerp(StartVolume3, 0, CurrentTime / PreviewLength);
            Debug.Log("Volume " + PreviewSource3.volume);
            yield return null;
        }
        Debug.Log("end preview at " + AudioSettings.dspTime);
        EndPreview3();
        yield break;
    }

    public void EndPreview1()
    {
        PreviewSource1.volume = StartVolume1;
        PreviewSource1.Stop();
    }

    public void EndPreview2()
    {
        PreviewSource2.volume = StartVolume2;
        PreviewSource2.Stop();
    }

    public void EndPreview3()
    {
        PreviewSource3.volume = StartVolume3;
        PreviewSource3.Stop();
    }

}
