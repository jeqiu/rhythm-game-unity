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
    [SerializeField] AudioSource PreviewSource;
    public AudioClip Preview;
    public float StartVolume;

    public void StartPreview()
    {
        //PreviewSource.clip = Preview;
        PreviewSource = GetComponent<AudioSource>();
        StartVolume = PreviewSource.volume;
        Debug.Log("Starting Volume " + PreviewSource.volume);
        StartCoroutine(PlaySongPreview());
    }

    IEnumerator PlaySongPreview()
    {
        float CurrentTime = 0;
        PreviewSource.Play();
        yield return new WaitForSeconds(15);
        while (CurrentTime < PreviewLength)
        {
            CurrentTime += Time.deltaTime;
            PreviewSource.volume = Mathf.Lerp(StartVolume, 0, CurrentTime / PreviewLength);
            Debug.Log("Volume " + PreviewSource.volume);
            yield return null;
        }
        Debug.Log("end preview at " + AudioSettings.dspTime);
        EndPreview();
        yield break;
    }

    public void EndPreview()
    {
        PreviewSource.volume = StartVolume;
        PreviewSource.Stop();
    }

}
