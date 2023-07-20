using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

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
    public bool CountDownRunning;

    // Start is called before the first frame update
    void Start()
    {
        CondInstance = this;

        CountDownRunning = false;
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
        if (Input.GetKeyDown("p"))
        {
            if (!Paused && MusicSource.isPlaying)
            {
                PausedSongTimestamp = (float)AudioSettings.dspTime;
                MusicSource.Pause();
                Paused = true;
                Debug.Log("Song Paused at " + AudioSettings.dspTime);
            }
            else if (Paused && !MusicSource.isPlaying)
            {
                CountDownRunning = true;
                Paused = false;
                StartCoroutine(PlayCountdown());
                PausedSongTimestamp = 0;
            }
        }

        if (Input.GetKeyDown("r"))
        {
            Restart();
        }

        //TODO: need to fix - still triggering when window not in focus
        if (!MusicSource.isPlaying && !Paused && MusicStarted && !CountDownRunning)
        {
            MusicStarted = false;
            Debug.Log("Song Ended at " + AudioSettings.dspTime);
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
        CountDownRunning = false;
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

    private void Restart()
    {
        //MusicSource.Stop();
        Debug.Log("Game Restarted at " + AudioSettings.dspTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
