using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalNote : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime; // time note should be tapped by player
    void Start()
    {
        timeInstantiated = CalConductor.GetMusicSourceTime();
        InternalGameLog.LogMessage("CalNote started at " + CalConductor.GetMusicSourceTime());
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: error with additional note created at instantiation
        // error removed if you use '- assignedTime + 0.9' instead of timeInstantiated but not optimal
        double timeSinceInstantiated = CalConductor.GetMusicSourceTime() - timeInstantiated; 
        float t = (float)(timeSinceInstantiated / (CalConductor.Instance.noteTime * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * CalConductor.Instance.NoteSpawnY, Vector3.up * CalConductor.Instance.NoteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
