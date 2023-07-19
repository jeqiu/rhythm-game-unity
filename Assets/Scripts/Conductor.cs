using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;

public class Conductor : MonoBehaviour
{
    public static Conductor CondInstance;
    //public Lane[] lanes;

    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SfxSource;

    [Header("Audio Clip")]
    public AudioClip Countdown;
    public AudioClip SongMusic;

    [Header("Note Settings")]
    public float NoteStartY;
    public float NoteBeatY;
    public float NoteEndY;
    public float NoteTapDiffTime; //time difference of tap from perfect beat
    public float PerfectThreshold;
    public float GoodThreshold;

    [Header("Other Settings/Variables")]
    public float SongDelay; //seconds
    public int InputDelay;
    public string MidiFilePath;

    //variables
    public static MidiFile SongMidi;
    public bool MusicStarted;
    public static bool Paused;
    public static float PausedSongTimestamp;
    public float SongTimestamp;
    public float DspSongPlayLength;

    // Start is called before the first frame update
    void Start()
    {
        CondInstance = this;
        
        MusicStarted = false;
        Paused = false;
        PausedSongTimestamp = 0;
        SongDelay = 3;
        SfxSource.clip = Countdown;
        MusicSource.clip = SongMusic;

        ReadMidiFile();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") && MusicStarted)
        {
            if(!Paused){
                PausedSongTimestamp = (float)AudioSettings.dspTime;
                MusicSource.Pause();
                Paused = true;
                Debug.Log("Song Paused");
            }
            else{
                StartCoroutine(PlayCountdown());
                Paused = false;
                PausedSongTimestamp = 0;
            }
        }

        
    }

    IEnumerator PlayCountdown(){
        SfxSource.Play();
        //SfxSource.PlayScheduled(AudioSettings.dspTime);
        Debug.Log("Countdown started at dspTime " + AudioSettings.dspTime);
        yield return new WaitForSeconds(SongDelay);
        PlaySong();
    }

    //Plays 'Ready' SFX and Song after delay
    //TODO: hoping this can also be used for resume
    private void PlaySong(){
        MusicSource.PlayScheduled(AudioSettings.dspTime);
        Debug.Log("Song played at dspTime " + AudioSettings.dspTime);

        MusicStarted = true;
    }

    // Read MidiFile from given filepath
    private void ReadMidiFile()
    {
        SongMidi = MidiFile.Read(Application.streamingAssetsPath + "/" + MidiFilePath);
        Debug.Log("Finished Reading Midi" + " at dspTime " + AudioSettings.dspTime);
        GetMidiData();
    }

    // Get Data from Midi file
    public void GetMidiData()
    {
        var SongNotes = SongMidi.GetNotes();
        var NoteArray = new Melanchall.DryWetMidi.Interaction.Note[SongNotes.Count];
        SongNotes.CopyTo(NoteArray, 0);

        //Invoke(nameof(PlaySong()));
        StartCoroutine(PlayCountdown());
    }

    //
    public static double GetMusicSourceTime(){
        return (double)CondInstance.MusicSource.timeSamples / CondInstance.MusicSource.clip.frequency;
    }
}
