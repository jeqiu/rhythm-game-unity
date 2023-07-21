using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime; // time note should be tapped by player
    void Start()
    {
        timeInstantiated = assignedTime - Conductor.Instance.noteTime;
        //timeInstantiated = Conductor.GetMusicSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = Conductor.GetMusicSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (Conductor.Instance.noteTime * 2));


        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * Conductor.Instance.NoteSpawnY, Vector3.up * Conductor.Instance.NoteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}