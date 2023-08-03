using System;
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
    public static Conductor Instance;
    public Lane[] lanes;
    public bool debug;
    public GameObject PausePanel;
    public GameObject SettingsPanel;
    public GameObject BackgroundFilter;
    public GameObject ReadyAnimation;

    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SfxSource;

    [Header("Audio Clip")]
    public AudioClip Countdown;
    public AudioClip SongMusic;
    public ScoreManager sfxVolumeScript;

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
    public float SongDelayInSeconds;
    public int InputDelayInMilliseconds;
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
        Instance = this;

        SharedData.debugMode = debug;
        CountDownRunning = false;
        MusicStarted = false;
        Paused = false;
        PausedSongTimestamp = 0;
        SongDelayInSeconds = 3;

        SfxSource.clip = Countdown;
        MusicSource.clip = SongMusic;
        SetVolume();
        SetDelay();

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
        if (Input.GetKeyDown("p"))
        {
            if (!Paused && MusicSource.isPlaying)
            {
                PauseGame();
            }
            else if (Paused && !MusicSource.isPlaying)
            {
                ResumeGame();
            }
        }

        if (Input.GetKeyDown("r"))
        {
            Restart();
        }

        if (Input.GetKeyDown("q"))
        {
            ReturnToMenu();
        }

        //TODO: need to fix - still triggering when window not in focus
        if (!MusicSource.isPlaying && !Paused && MusicStarted && !CountDownRunning)
        {
            MusicStarted = false;
            InternalGameLog.LogMessage("Song Ended at " + AudioSettings.dspTime);
            //SceneManager.LoadScene("ScoreBoard");
            Invoke("GoToScoreBoard", 2.0f);
        }

        }

    IEnumerator PlayCountdown(){
        Instantiate(ReadyAnimation);
        SfxSource.Play();
        //SfxSource.PlayScheduled(AudioSettings.dspTime);
        InternalGameLog.LogMessage("Countdown started at dspTime " + AudioSettings.dspTime);
        yield return new WaitForSeconds(SongDelayInSeconds);
        PlaySong();
    }

    //Plays 'Ready' SFX and Song after delay
    private void PlaySong(){
        MusicSource.PlayScheduled(AudioSettings.dspTime);
        InternalGameLog.LogMessage("Song played at dspTime " + AudioSettings.dspTime);

        MusicStarted = true;
        CountDownRunning = false;
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

        foreach (var lane in lanes) lane.SetTimeStamps(NoteArray);
      
        //Invoke(nameof(StartSong), SongDelayInSeconds);
        StartCoroutine(PlayCountdown());
    }

    //redundant - choose one
    public static double GetMusicSourceTime()
    {
        return (double)Instance.MusicSource.timeSamples / Instance.MusicSource.clip.frequency;
    }

    public void PauseGame()
    {
        StopAllCoroutines();
        PausedSongTimestamp = (float)AudioSettings.dspTime;
        MusicSource.Pause();
        Paused = true;
        InternalGameLog.LogMessage("Song Paused at " + AudioSettings.dspTime);
        PausePanel.SetActive(true);
        BackgroundFilter.SetActive(true);
    }

    public void ResumeGame()
    {
        CountDownRunning = true;
        Paused = false;
        SetVolume();
        sfxVolumeScript.SetSfxVolume();
        StartCoroutine(PlayCountdown());
        PausedSongTimestamp = 0;
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        BackgroundFilter.SetActive(false);
    }

    public void Restart()
    {
        //MusicSource.Stop();
        InternalGameLog.LogMessage("Game Restarted at " + AudioSettings.dspTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenSettings()
    {
        PauseGame();
        SettingsPanel.SetActive(true);
        PausePanel.SetActive(false);
    }

    private void SetVolume()
    {
        SfxSource.volume = SharedData.sfxVolume;
        MusicSource.volume = SharedData.musicVolume;
    }

    private void SetDelay()
    {
        InputDelayInMilliseconds = SharedData.inputDelay;
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void GoToScoreBoard()
    {
        SceneManager.LoadScene("ScoreBoard");
    }
}
