using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class CalConductor : MonoBehaviour
{
    public static CalConductor Instance;
    public CalLane lane;
    //public CalLane[] lanes;
    public bool debug;
    //private Melanchall.DryWetMidi.Interaction.Note[] noteArray;

    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;

    [Header("Audio Clip")]
    public AudioClip SongMusic;
    //public ScoreManager sfxVolumeScript;

    [Header("Note Settings")]
    public float NoteSpawnY;
    public float NoteBeatY;
    public float NoteDespawnY
    {
        get
        {
            return NoteBeatY - (NoteSpawnY - NoteBeatY);
        }
    }

    public float noteTime; // time note will be on screen
    public float NoteTapDiffTime; //time difference of tap from perfect beat
    public double PerfectHitThreshold;// in seconds
    public double GoodHitThreshold;// in seconds

    [Header("Other Settings/Variables")]
    public int InputDelayInMilliseconds;
    public string MidiFilePath;

    //variables
    public static MidiFile SongMidi;
    public static float PausedSongTimestamp;
    public float SongTimestamp;
    public float DspSongPlayLength;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        SharedData.debugMode = debug;
        PausedSongTimestamp = 0;

        MusicSource.clip = SongMusic;
        //SetVolume();

        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadMidiFile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            ReturnToMenu();
        }

        if(!MusicSource.isPlaying){
            if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
            {
                StartCoroutine(ReadFromWebsite());
            }
            else
            {
                ReadMidiFile();
            }
        }
    }

    //Plays 'Ready' SFX and Song after delay
    private void PlaySong(){
        MusicSource.PlayScheduled(AudioSettings.dspTime);
        InternalGameLog.LogMessage("Song played at dspTime " + AudioSettings.dspTime);
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + MidiFilePath + ".mid"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                InternalGameLog.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    SongMidi = MidiFile.Read(stream);
                    GetMidiData();
                }
            }
        }
    }

    // Read MidiFile from given filepath
    private void ReadMidiFile()
    {
        SongMidi = MidiFile.Read(Application.streamingAssetsPath + "/" + MidiFilePath + ".mid");
        InternalGameLog.LogMessage("Finished Reading Midi" + " at dspTime " + AudioSettings.dspTime);
        GetMidiData();
    }

    // Get Data from Midi file
    public void GetMidiData() 
    {
        var SongNotes = SongMidi.GetNotes();
        var NoteArray = new Melanchall.DryWetMidi.Interaction.Note[SongNotes.Count];
        SongNotes.CopyTo(NoteArray, 0);
        
        InternalGameLog.LogMessage("There are total notes: " + SongNotes.Count);
        InternalGameLog.LogMessage("In NoteArray, there are " + NoteArray.Length);

        //foreach (var lane in lanes) lane.SetTimeStamps(NoteArray);
        lane.SetTimeStamps(NoteArray);
        PlaySong();
    }

    //redundant - choose one
    public static double GetMusicSourceTime()
    {
        return (double)Instance.MusicSource.timeSamples / Instance.MusicSource.clip.frequency;
    }

    public void SetDelay()
    {
        InputDelayInMilliseconds = SharedData.inputDelay;
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
