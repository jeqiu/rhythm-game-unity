using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public GameObject visualGuide;
    SpriteRenderer visualSprite;
    private Color visualColor;
    public Color missColor = new Color(1, 0.2f, 0.2f);
    public Color hitColor = new Color(0.4f, 0.7f, 0.9f);  //nice light blue 0.1f, 0.8f, 1
    public Color perfectHitColor = new Color(0, 1, 0); //darker green 0.1f, 0.7f, 0.3f

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>(); // each tap needs a timestamp

    int spawnIndex = 0;
    int inputIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        visualSprite = visualGuide.GetComponent<SpriteRenderer>();
        visualColor = visualSprite.color;
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, Conductor.SongMidi.GetTempoMap());
                timeStamps.Add((double)(metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f));   
            }
        }
        Debug.Log("In this lane there are " + timeStamps.Count + " notes");
        SharedData.maxScore += timeStamps.Count*2;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (Conductor.GetMusicSourceTime() >= timeStamps[spawnIndex] - Conductor.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double GoodHitThreshold = Conductor.Instance.GoodHitThreshold;
            double PerfectHitThreshold = Conductor.Instance.PerfectHitThreshold;
            double audioTime = Conductor.GetMusicSourceTime() - (Conductor.Instance.InputDelayInMilliseconds / 1000.0);

            //for audio sync testing
            if ((Math.Abs(audioTime - timeStamp) < 0.025) && SharedData.debugMode && Conductor.Instance.MusicStarted && !Conductor.Paused)
            {
                //Beat();
                PerfectHit();
                Destroy(notes[inputIndex].gameObject);
                inputIndex++;
            }
            //

            if (Input.GetKeyDown(input))
            {
                //StartCoroutine(AnimateKeyPress(new Color(0.4f, 0.7f, 0.9f)));
                if (Math.Abs(audioTime - timeStamp) < PerfectHitThreshold)
                {
                    PerfectHit();
                    print($"Perfect hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else if (Math.Abs(audioTime - timeStamp) < GoodHitThreshold)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    StartCoroutine(AnimateKeyPress(missColor));
                }
            }

            if (timeStamp + GoodHitThreshold <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }

    }
    private void Hit()
    {
        StartCoroutine(AnimateKeyPress(hitColor));
        ScoreManager.Hit(); // sound effects/visual on hit
    }
    private void PerfectHit()
    {
        StartCoroutine(AnimateKeyPress(perfectHitColor));
        ScoreManager.PerfectHit(); // sound effects/visual on hit
    }
    private void Miss()
    {
        ScoreManager.Miss();
    }
    private void Beat()
    {
        ScoreManager.Beat();
    }
    private IEnumerator AnimateKeyPress(Color cr)
    {
        Color pressedColor = cr; // new Color(1, 0, 0);
        pressedColor.a = visualColor.a; //Set Alpha to match original colors
        visualSprite.color = pressedColor; 
        float animSpeedInSec = 0.2f;
        float counter = 0;

        //Fade Back
        while (counter < animSpeedInSec)
        {
            counter += Time.deltaTime;
            visualSprite.color = Color.Lerp(pressedColor, visualColor, counter / animSpeedInSec);
            yield return null;
        }

        yield break;
    }
}